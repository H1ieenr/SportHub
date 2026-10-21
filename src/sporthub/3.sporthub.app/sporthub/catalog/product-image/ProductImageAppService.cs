using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shared.Common;
using Shared.Exceptions;
using sporthub.app.contracts;
using sporthub.domain;

namespace sporthub.app
{
    public class ProductImageAppService : IProductImageAppService
    {
        private static readonly string[] AllowedImageTypes = ["image/jpeg", "image/png", "image/webp"];
        private const long MaxImageSize = 5 * 1024 * 1024;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly string _folderName = "Catalog/Product/";
        public ProductImageAppService(IProductImageRepository productImageRepository, IProductRepository productRepository, 
            IProductVariantRepository productVariantRepository, ISportHubUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _productImageRepository = productImageRepository;
            _productRepository = productRepository;
            _productVariantRepository = productVariantRepository;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }
        #region admin
        public async Task<OperationResult<ProductImageDTO>> CreateAsync(CreateProductImageRequestDTO model, CancellationToken cancellationToken = default)
        {
            var folder = model.product_variant_id is null
                ? $"{_folderName}{model.product_id}"
                : $"{_folderName}{model.product_id}/{model.product_variant_id}";

            var upload = await _cloudinaryService.UploadImageAsync(model.file_image, folder);
            if (!upload.IsSuccess)
                throw new BadRequestException("Upload ảnh thất bại");

            try
            {
                var productImage = ProductImage.Create(model.product_id, model.display_order,
                    model.is_primary, model.product_variant_id);
                productImage.UpdateImage(upload.Url, upload.PublicId);

                await _productImageRepository.CreateAsync(productImage);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return OperationResult<ProductImageDTO>.Success(_mapper.Map<ProductImageDTO>(productImage));
            }
            catch
            {
                await _cloudinaryService.DeleteImageAsync(upload.PublicId);
                throw;
            }
        }
        public async Task<OperationResult<List<CreateBatchProductImageResponseDTO>>> CreateBatchAsync(
                 CreateBatchProductImageRequestDTO model, CancellationToken cancellationToken = default)
        {
            var items = model.product_images;
            if (items.Count == 0)
                return OperationResult<List<CreateBatchProductImageResponseDTO>>.Success([]);
            if (items.Count > 10)
                throw new BadRequestException("Tối đa 10 ảnh mỗi lần.");
            if (items.Count(x => x.is_primary) > 1)
                throw new BadRequestException("Chỉ được chọn tối đa 1 ảnh primary");

            // kiểm tra hết trước khi upload
            await EnsureImageScopeAsync(model.product_id, model.product_variant_id, cancellationToken);
            foreach (var item in items) ValidateImageFile(item.file_image);

            var siblings = await _productImageRepository
                .GetTrackedByScopeAsync(model.product_id, model.product_variant_id, cancellationToken);

            var folder = model.product_variant_id is null
                ? $"{_folderName}{model.product_id}"
                : $"{_folderName}{model.product_id}/{model.product_variant_id}";

            // upload song song, tối đa 3 ảnh cùng lúc
            using var semaphore = new SemaphoreSlim(3);
            var tasks = items.Select(async item =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try { return (item, upload: await _cloudinaryService.UploadImageAsync(item.file_image, folder)); }
                finally { semaphore.Release(); }
            }).ToList();

            try
            {
                await Task.WhenAll(tasks);
            }
            catch
            {
                // có task lỗi/bị hủy: dọn các ảnh đã upload thành công rồi mới ném lỗi
                await DeleteImagesQuietlyAsync(tasks
                    .Where(t => t.IsCompletedSuccessfully && t.Result.upload.IsSuccess)
                    .Select(t => t.Result.upload.PublicId));
                throw;
            }

            var rows = new List<(ProductImage? entity, CreateBatchProductImageResponseDTO dto)>();
            foreach (var (item, upload) in tasks.Select(t => t.Result))
            {
                var fileName = item.file_image.FileName;
                if (!upload.IsSuccess)
                {
                    rows.Add((null, CreateBatchProductImageResponseDTO.Fail(fileName, "Upload ảnh thất bại")));
                    continue;
                }

                var entity = ProductImage.Create(model.product_id, item.display_order, item.is_primary, model.product_variant_id);
                entity.UpdateImage(upload.Url, upload.PublicId);
                rows.Add((entity, CreateBatchProductImageResponseDTO.Ok(fileName, upload.Url)));
            }

            var entities = rows.Where(x => x.entity is not null).Select(x => x.entity!).ToList();
            if (entities.Count > 0)
            {
                // chưa có ảnh chính nào trong phạm vi và batch không chọn: ảnh đầu tiên làm ảnh chính
                if (!entities.Any(e => e.is_primary) && !siblings.Any(s => s.is_primary))
                    entities[0].SetPrimary(true);

                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    if (entities.Any(e => e.is_primary))
                    {
                        foreach (var s in siblings.Where(x => x.is_primary))
                            s.SetPrimary(false);
                        await _unitOfWork.SaveChangesAsync(cancellationToken);   // bỏ primary cũ trước
                    }

                    await _productImageRepository.CreateBatchAsync(entities, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);       // rồi mới thêm ảnh mới

                    await _unitOfWork.CommitTransactionAsync(cancellationToken);
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);
                    await DeleteImagesQuietlyAsync(entities.Select(e => e.image_public_id));
                    throw;
                }
            }

            foreach (var (entity, dto) in rows)
                if (entity is not null) dto.id = entity.id;

            return OperationResult<List<CreateBatchProductImageResponseDTO>>.Success(rows.Select(x => x.dto).ToList());
        }
        public async Task<OperationResult<bool>> DeleteAsync(DeleteProductImageRequestDTO model, CancellationToken cancellationToken = default)
        {
            var productImage = await _productImageRepository.ProductImageGetByIdAsync(model.id, cancellationToken)
                    ?? throw new NotFoundException("Không tìm thấy ảnh.");

            if (productImage.is_primary)
            {
                var siblings = await _productImageRepository.GetTrackedByScopeAsync(
                    productImage.product_id, productImage.product_variant_id, cancellationToken);

                // chỉ chặn khi còn ảnh khác để chuyển primary sang
                if (siblings.Count > 1)
                    throw new ConflictException("Ảnh đang là ảnh chính, hãy chọn ảnh chính khác trước khi xóa.");
            }

            var publicId = productImage.image_public_id;

            await _productImageRepository.DeleteAsync(productImage, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await DeleteImagesQuietlyAsync([publicId]);

            return OperationResult<bool>.Success(true, "Xóa ảnh thành công");
        }
        public async Task<OperationResult<bool>> UpdatePrimaryProductImageAsync(UpdatePrimaryProductImageRequestDTO model, CancellationToken cancellationToken = default)
        {
            ProductImage product_image = await _productImageRepository.ProductImageGetByIdAsync(model.id, cancellationToken);
            if (product_image == null) throw new NotFoundException("Không tìm thấy sản phẩm.");

            ProductImage product_image_primary = await _productImageRepository.ProductImageGetByPrimaryAsync(model.product_id, model.product_variant_id, cancellationToken);
            if (product_image_primary != null)
            {
                product_image_primary.Deprimary();
                _productImageRepository.UpdateAsync(product_image_primary);
            }

            if (product_image.is_primary) product_image.Deprimary(); else product_image.Primary();
            _productImageRepository.UpdateAsync(product_image);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Cập nhật ảnh chính thành công");
        }
        public async Task<OperationResult<bool>> UpdateDisplayOrderProductImageAsync(UpdateDisplayOrderProductImageRequestDTO model, CancellationToken cancellationToken = default)
        {
            var images = await _productImageRepository.ProductImageGetNoPagingAsync(model.product_id, model.product_variant_id, null, cancellationToken);

            var requestedIds = model.items.Select(x => x.id).ToHashSet();
            if (requestedIds.Count != model.items.Count || !images.Select(x => x.id).ToHashSet().SetEquals(requestedIds))
                throw new BadRequestException("Danh sách ảnh không khớp với ảnh hiện có.");

            var newOrder = model.items
                .OrderBy(x => x.display_order)
                .Select((x, index) => (x.id, index))
                .ToDictionary(x => x.id, x => x.index);

            foreach (var img in images)
                img.UpdateOrder(newOrder[img.id]);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult<bool>.Success(true, "Cập nhật vị trí ảnh thành công");
        }
        public async Task<OperationResult<List<ProductImageDTO>>> ProductImageGetNoPagingAsync(GetProductImageNoPagingRequestDTO model, CancellationToken cancellationToken = default)
        {
            var items = await _productImageRepository.ProductImageGetNoPagingAsync(model.product_id, model.product_variant_id, model.is_primary, cancellationToken);
            List<ProductImageDTO> dto = _mapper.Map<List<ProductImageDTO>>(items);
            return OperationResult<List<ProductImageDTO>>.Success(dto);
        }
        #endregion

        private static void ValidateImageFile(IFormFile file)
        {
            if (!AllowedImageTypes.Contains(file.ContentType))
                throw new BadRequestException($"'{file.FileName}': chỉ chấp nhận ảnh jpeg, png, webp.");
            if (file.Length > MaxImageSize)
                throw new BadRequestException($"'{file.FileName}': ảnh tối đa 5MB.");
        }

        private async Task EnsureImageScopeAsync(long productId, long? variantId, CancellationToken ct)
        {
            var product = await _productRepository.ProductGetByIdAsync(productId, ct)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm.");
            if (variantId is null) return;

            var variant = await _productVariantRepository.ProductVariantGetByIdAsync(variantId.Value, ct)
                ?? throw new NotFoundException("Không tìm thấy biến thể.");
            if (variant.product_id != product.id)
                throw new BadRequestException("Biến thể không thuộc sản phẩm này.");
        }

        private async Task DeleteImagesQuietlyAsync(IEnumerable<string?> publicIds)
        {
            foreach (var id in publicIds.Where(x => !string.IsNullOrEmpty(x)))
            {
                try { await _cloudinaryService.DeleteImageAsync(id!); }
                catch { }
            }
        }
    }
}