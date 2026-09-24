using sporthub.app.contracts;
using sporthub.domain;
using Shared.Common;
using AutoMapper;
using Shared.Exceptions;

namespace sporthub.app
{
    public class ProductVariantAppService : IProductVariantAppService
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductVariantAppService(IProductVariantRepository productVariantRepository, ISportHubUnitOfWork unitOfWork, IMapper mapper)
        {
            _productVariantRepository = productVariantRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region admin
        public async Task<OperationResult<ProductVariantDTO>> CreateAsync(CreateProductVariantRequestDTO model, CancellationToken cancellationToken = default)
        {
            if (await _productVariantRepository.ExistsBySkuAsync(model.sku, null, cancellationToken))
                throw new ConflictException("Sku đã tồn tại.");

            ProductVariant product_variant = ProductVariant.Create(model.product_id, model.sku, model.name, model.cost_price,
                                        model.sale_price, model.value_json, model.is_active);

            await _productVariantRepository.CreateAsync(product_variant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            ProductVariantDTO dto = _mapper.Map<ProductVariantDTO>(product_variant);
            return OperationResult<ProductVariantDTO>.Success(dto);
        }
        public async Task<OperationResult<List<CreateBatchProductVariantResponseDTO>>> CreateBatchAsync(
                 CreateBatchProductVariantRequestDTO model, CancellationToken cancellationToken = default)
        {
            var items = model.product_variants;
            if (items.Count == 0) return OperationResult<List<CreateBatchProductVariantResponseDTO>>.Success([]);

            var skus = items.Select(x => x.sku).ToList();
            var duplicated = skus.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToHashSet();
            var existing = (await _productVariantRepository.GetExistingSkusAsync(skus, cancellationToken)).ToHashSet();

            string? GetError(string sku) => duplicated.Contains(sku) ? "Sku bị trùng trong danh sách gửi lên" : existing.Contains(sku) ? "Sku đã tồn tại" : null;

            var rows = items.Select(item =>
            {
                var error = GetError(item.sku);
                var entity = error is null ? ProductVariant.Create(model.product_id, item.sku, item.name, item.cost_price, item.sale_price, item.value_json, item.is_active) : null;
                var dto = error is null ? CreateBatchProductVariantResponseDTO.Ok(item.sku, item.name) : CreateBatchProductVariantResponseDTO.Fail(item.sku, item.name, error);

                return (entity, dto);
            }).ToList();

            var entities = rows.Where(x => x.entity is not null).Select(x => x.entity!).ToList();
            if (entities.Count > 0)
            {
                await _productVariantRepository.CreateBatchAsync(entities);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            foreach (var (entity, dto) in rows) if (entity is not null) dto.id = entity.id;

            return OperationResult<List<CreateBatchProductVariantResponseDTO>>.Success(rows.Select(x => x.dto).ToList());
        }
        public async Task<OperationResult<ProductVariantDTO>> UpdateAsync(UpdateProductVariantRequestDTO model, CancellationToken cancellationToken = default)
        {
            ProductVariant product_variant = await _productVariantRepository.ProductVariantGetByIdAsync(model.id, cancellationToken);
            if (product_variant == null) throw new NotFoundException("Không tìm thấy danh mục.");

            if (await _productVariantRepository.ExistsBySkuAsync(model.sku, model.id, cancellationToken))
                throw new ConflictException("Sku đã tồn tại.");

            product_variant.Update(model.name, model.cost_price,
                                   model.sale_price, model.value_json, model.is_active);

            _productVariantRepository.UpdateAsync(product_variant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            ProductVariantDTO dto = _mapper.Map<ProductVariantDTO>(product_variant);
            return OperationResult<ProductVariantDTO>.Success(dto);
        }

        public async Task<OperationResult<bool>> DeleteAsync(DeleteProductVariantRequestDTO model, CancellationToken cancellationToken = default)
        {
            ProductVariant product_variant = await _productVariantRepository.ProductVariantGetByIdAsync(model.id, cancellationToken);
            if (product_variant == null) throw new NotFoundException("Không tìm thấy sản phẩm.");

            product_variant.Deactivate();
            _productVariantRepository.DeleteAsync(product_variant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Xóa sản phẩm thành công");
        }
        public async Task<OperationResult<bool>> UpdateActiveProductVariantAsync(UpdateActiveProductVariantRequestDTO model, CancellationToken cancellationToken = default)
        {
            ProductVariant product_variant = await _productVariantRepository.ProductVariantGetByIdAsync(model.id, cancellationToken);
            if (product_variant == null) throw new NotFoundException("Không tìm thấy sản phẩm.");

            if (product_variant.is_active) product_variant.Deactivate(); else product_variant.Activate();

            _productVariantRepository.UpdateAsync(product_variant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true, "Cập nhật trạng thái thành công");
        }
        public async Task<OperationResult<ProductVariantDTO>> ProductVariantGetByIdAsync(ProductVariantGetByIdRequestDTO model, CancellationToken cancellationToken = default)
        {
            ProductVariant product_variant = await _productVariantRepository.ProductVariantGetByIdAsync(model.id, cancellationToken);
            if (product_variant == null) throw new NotFoundException("Không tìm thấy sản phẩm.");
            ProductVariantDTO dto = _mapper.Map<ProductVariantDTO>(product_variant);

            return OperationResult<ProductVariantDTO>.Success(dto);
        }
        public async Task<OperationResult<PagedResult<ProductVariantDTO>>> ProductVariantGetPagedAsync(GetProductVariantPagedRequestDTO model, CancellationToken cancellationToken = default)
        {
            var (items, total) = await _productVariantRepository.ProductVariantGetPagedAsync(
                model.page_number,
                model.page_size,
                model.search_text,
                model.sort_by,
                model.sort_dir,
                model.product_id,
                model.is_active,
                cancellationToken);

            var pagedResult = new PagedResult<ProductVariant>(items, total, model.page_number, model.page_size);
            var dtoResult = pagedResult.MapTo(_mapper.Map<ProductVariantDTO>);

            return OperationResult<PagedResult<ProductVariantDTO>>.Success(dtoResult);
        }
        #endregion
        public async Task<OperationResult<List<ProductVariantDTO>>> ProductVariantGetNoPagingAsync(GetProductVariantNoPagingRequestDTO model, CancellationToken cancellationToken = default)
        {
            var items = await _productVariantRepository.ProductVariantGetNoPagingAsync(model.search_text, model.product_id, model.is_active, cancellationToken);
            List<ProductVariantDTO> dto = _mapper.Map<List<ProductVariantDTO>>(items);
            return OperationResult<List<ProductVariantDTO>>.Success(dto);
        }
    }
}