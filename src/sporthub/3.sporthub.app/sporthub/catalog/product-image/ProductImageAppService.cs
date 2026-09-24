using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using AutoMapper;
using Shared.Exceptions;

namespace sporthub.app
{
    public class ProductImageAppService : IProductImageAppService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        private readonly string _folderName = "";
        public ProductImageAppService(IProductImageRepository productImageRepository, ISportHubUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _productImageRepository = productImageRepository;
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

            if (items.Count(x => x.is_primary) > 1)
                throw new BadRequestException("Chỉ được chọn tối đa 1 ảnh primary");

            var folder = model.product_variant_id is null
                 ? $"{_folderName}{model.product_id}"
                 : $"{_folderName}{model.product_id}/{model.product_variant_id}";

            using var semaphore = new SemaphoreSlim(3);
            var uploads = await Task.WhenAll(items.Select(async item =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try { return (item, upload: await _cloudinaryService.UploadImageAsync(item.file_image, folder)); }
                finally { semaphore.Release(); }
            }));

            (ProductImage? entity, CreateBatchProductImageResponseDTO dto) Build(CreateBatchProductImageItem item, dynamic upload)
            {
                var fileName = item.file_image.FileName;
                if (!upload.IsSuccess)
                    return (null, CreateBatchProductImageResponseDTO.Fail(fileName, "Upload ảnh thất bại"));

                var entity = ProductImage.Create(model.product_id, item.display_order, item.is_primary, model.product_variant_id);
                entity.UpdateImage(upload.Url, upload.PublicId);
                return (entity, CreateBatchProductImageResponseDTO.Ok(fileName, upload.Url));
            }

            var rows = uploads.Select(x => Build(x.item, x.upload)).ToList();

            var entities = rows.Where(x => x.entity is not null).Select(x => x.entity!).ToList();
            if (entities.Count > 0)
            {
                try
                {
                    await _productImageRepository.CreateBatchAsync(entities);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                catch
                {
                    await Task.WhenAll(entities.Select(e => _cloudinaryService.DeleteImageAsync(e.image_public_id)));
                    throw;
                }
            }

            foreach (var (entity, dto) in rows)
                if (entity is not null) dto.id = entity.id;

            return OperationResult<List<CreateBatchProductImageResponseDTO>>.Success(rows.Select(x => x.dto).ToList());
        }
        public async Task<OperationResult<bool>> DeleteAsync(DeleteProductImageRequestDTO model, CancellationToken cancellationToken = default)
        {
            ProductImage product_image = await _productImageRepository.ProductImageGetByIdAsync(model.id, cancellationToken);
            if (product_image == null) throw new NotFoundException("Không tìm thấy sản phẩm.");
            if (product_image.is_primary) throw new NotFoundException("Ảnh đang là ảnh chính không thể xoá");

            await _cloudinaryService.DeleteImageAsync(product_image.image_public_id);

            _productImageRepository.DeleteAsync(product_image);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

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
    }
}