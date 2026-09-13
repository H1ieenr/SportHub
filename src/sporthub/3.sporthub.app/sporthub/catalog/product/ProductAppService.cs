using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using AutoMapper;
using Shared.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace sporthub.app
{
    public class ProductAppService : IProductAppService
    {
        private readonly IProductRepository _productRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;
        public ProductAppService(IProductRepository productRepository, ISportHubUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IMapper mapper)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }
        #region admin
        public async Task<OperationResult<ProductDTO>> CreateAsync(CreateProductRequestDTO model, CancellationToken cancellationToken = default)
        {
            if (await _productRepository.ExistsBySlugAsync(model.slug, null, cancellationToken))
                throw new ConflictException("Slug đã tồn tại.");

            Product product = Product.Create(model.category_id, model.name, model.slug, model.base_price,
                                        model.brand_id, model.description, model.is_featured, model.status);

            await _productRepository.CreateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            ProductDTO dto = _mapper.Map<ProductDTO>(product);
            return OperationResult<ProductDTO>.Success(dto);
        }

        public async Task<OperationResult<ProductDTO>> UpdateAsync(UpdateProductRequestDTO model, CancellationToken cancellationToken = default)
        {
            Product product = await _productRepository.ProductGetByIdAsync(model.id, cancellationToken);
            if (product == null) throw new NotFoundException("Không tìm thấy danh mục.");

            if (await _productRepository.ExistsBySlugAsync(model.slug, model.id, cancellationToken))
                throw new ConflictException("Slug đã tồn tại.");

            product.Update(model.category_id, model.name, model.slug, model.base_price,
                                       model.brand_id, model.description, model.is_featured, model.status);

            _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            ProductDTO dto = _mapper.Map<ProductDTO>(product);
            return OperationResult<ProductDTO>.Success(dto);
        }

        public async Task<OperationResult<bool>> DeleteAsync(DeleteProductRequestDTO model, CancellationToken cancellationToken = default)
        {
            Product product = await _productRepository.ProductGetByIdAsync(model.id, cancellationToken);
            if (product == null) throw new NotFoundException("Không tìm thấy danh mục.");

            if (product.variants.Any())
                throw new ConflictException("Không thể xóa sản phẩm đang có biến thể");

            _productRepository.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Xóa danh mục thành công");
        }
        public async Task<OperationResult<bool>>UpdateFeaturedAsync(UpdateFeaturedProductRequestDTO model, CancellationToken cancellationToken = default)
        {
            Product product = await _productRepository.ProductGetByIdAsync(model.id, cancellationToken);
            if (product == null) throw new NotFoundException("Không tìm thấy danh mục.");

            if (product.is_featured) product.Defeature(); else product.Feature();

            _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Cập nhật trạng thái thành công");
        }
        public async Task<OperationResult<ProductDTO>> ProductGetByIdAsync(ProductGetByIdRequestDTO model, CancellationToken cancellationToken = default)
        {
            Product Product = await _productRepository.ProductGetByIdAsync(model.id, cancellationToken);
            if (Product == null) throw new NotFoundException("Không tìm thấy danh mục.");
            ProductDTO dto = _mapper.Map<ProductDTO>(Product);

            return OperationResult<ProductDTO>.Success(dto);
        }
        public async Task<OperationResult<PagedResult<ProductDTO>>> ProductGetPagedAsync(GetProductPagedRequestDTO model, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _productRepository.ProductGetPagedAsync(
                model.page_number,
                model.page_size,
                model.search_text,
                model.sort_by,
                model.sort_dir,
                model.is_featured,
                model.status,
                cancellationToken);

            var pagedResult = new PagedResult<Product>(items, total, model.page_number, model.page_size);
            var dtoResult = pagedResult.MapTo(_mapper.Map<ProductDTO>);

            return OperationResult<PagedResult<ProductDTO>>.Success(dtoResult);
        }
        #endregion
        public async Task<OperationResult<List<ProductDTO>>> ProductGetNoPagingAsync(GetProductNoPagingRequestDTO model, CancellationToken cancellationToken = default)
        {
            var items = await _productRepository.ProductGetNoPagingAsync(model.search_text, model.is_featured, model.status, cancellationToken);
            List<ProductDTO> dto = _mapper.Map<List<ProductDTO>>(items);
            return OperationResult<List<ProductDTO>>.Success(dto);
        }

    }
}