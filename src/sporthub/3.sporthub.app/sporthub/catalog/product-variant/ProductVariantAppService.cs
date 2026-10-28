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
        private readonly IProductRepository _productRepository;
        private readonly ICategoryOptionRepository _categoryOptionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOptionValueRepository _optionValueRepository;
        private readonly IProductVariantOptionValueRepository _productVariantOptionValueRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly ISportHubUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductVariantAppService(IProductVariantRepository productVariantRepository, IProductRepository productRepository, ICategoryOptionRepository categoryOptionRepository,
                ICategoryRepository categoryRepository, IOptionValueRepository optionValueRepository, IProductVariantOptionValueRepository productVariantOptionValueRepository,
                IInventoryItemRepository inventoryItemRepository, IInventoryTransactionRepository inventoryTransactionRepository, ISportHubUnitOfWork unitOfWork, IMapper mapper)
        {
            _productVariantRepository = productVariantRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _categoryOptionRepository = categoryOptionRepository;
            _categoryRepository = categoryRepository;
            _optionValueRepository = optionValueRepository;
            _productVariantOptionValueRepository = productVariantOptionValueRepository;
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _mapper = mapper;
        }
        #region admin
        public async Task<OperationResult<ProductVariantDTO>> CreateAsync(CreateProductVariantRequestDTO model, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.ProductGetByIdAsync(model.product_id, cancellationToken)
                        ?? throw new NotFoundException("Không tìm thấy sản phẩm.");

            var category = await _categoryRepository.CategoryGetByIdAsync(product.category_id, cancellationToken)
                        ?? throw new NotFoundException("Không tìm thấy danh mục.");

            var effectiveCategoryId = category.parent_id ?? category.id;

            var allowedOptions = await _categoryOptionRepository.CategoryOptionGetTrackedByCategoryIdAsync(effectiveCategoryId, cancellationToken);
            var allowedDefIds = allowedOptions.Select(x => x.option_definition_id).ToHashSet();

            var submittedDefIds = model.option_values.Select(x => x.option_definition_id).ToList();

            var dup = submittedDefIds.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
            if (dup.Count > 0)
                throw new ConflictException($"option_definition_id bị trùng: {string.Join(", ", dup)}");

            var missing = allowedDefIds.Except(submittedDefIds).ToList();
            if (missing.Count > 0)
                throw new BadRequestException("Thiếu giá trị cho một hoặc nhiều option của danh mục.");

            var extra = submittedDefIds.Except(allowedDefIds).ToList();
            if (extra.Count > 0)
                throw new BadRequestException("Có option không thuộc danh mục này.");

            var valueIds = model.option_values.Select(x => x.option_value_id).ToList();
            var values = await _optionValueRepository.OptionValueGetByIdsAsync(valueIds, cancellationToken);
            foreach (var ov in model.option_values)
            {
                var val = values.FirstOrDefault(v => v.id == ov.option_value_id)
                    ?? throw new NotFoundException($"option_value_id {ov.option_value_id} không tồn tại.");
                if (val.option_definition_id != ov.option_definition_id)
                    throw new BadRequestException($"option_value_id {ov.option_value_id} không thuộc option_definition_id {ov.option_definition_id}.");
            }

            var sku = model.sku.Trim().ToUpperInvariant();
            if (await _productVariantRepository.ExistsBySkuAsync(sku, null, cancellationToken))
                throw new ConflictException($"SKU '{sku}' đã tồn tại.");

            var existingVariants = await _productVariantRepository.ProductVariantGetByProductIdWithOptionsAsync(product.id, cancellationToken);
            var newCombo = valueIds.ToHashSet();
            var isDuplicateCombo = existingVariants.Any(v =>
                v.option_values.Select(x => x.option_value_id).ToHashSet().SetEquals(newCombo));
            if (isDuplicateCombo)
                throw new ConflictException("Đã tồn tại biến thể với tổ hợp option này.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var variant = ProductVariant.Create(product.id, sku, model.name, model.cost_price, model.sale_price, null, model.is_active);
                await _productVariantRepository.CreateAsync(variant, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                foreach (var ov in model.option_values)
                {
                    var entity = ProductVariantOptionValue.Create(variant.id, ov.option_definition_id, ov.option_value_id);
                    await _productVariantOptionValueRepository.CreateAsync(entity, cancellationToken);
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await CreateInventoryAsync(variant, model.initial_quantity, model.reorder_level, cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return OperationResult<ProductVariantDTO>.Success(_mapper.Map<ProductVariantDTO>(variant));
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
        public async Task<OperationResult<List<CreateBatchProductVariantResponseDTO>>> CreateBatchAsync(CreateBatchProductVariantRequestDTO model, CancellationToken cancellationToken = default)
        {
            var items = model.product_variants;
            if (items.Count == 0)
                return OperationResult<List<CreateBatchProductVariantResponseDTO>>.Success([]);
            if (items.Count > 100)
                throw new BadRequestException("Tối đa 100 biến thể mỗi lần.");

            var product = await _productRepository.ProductGetByIdAsync(model.product_id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy sản phẩm.");
            var category = await _categoryRepository.CategoryGetByIdAsync(product.category_id, cancellationToken)
                ?? throw new NotFoundException("Không tìm thấy danh mục.");
            var effectiveCategoryId = category.parent_id ?? category.id;

            var allowedOptions = await _categoryOptionRepository.CategoryOptionGetTrackedByCategoryIdAsync(effectiveCategoryId, cancellationToken);
            var allowedDefIds = allowedOptions.Select(x => x.option_definition_id).ToHashSet();

            var normalized = items.Select(x => (item: x, sku: x.sku.Trim().ToUpperInvariant())).ToList();
            var skus = normalized.Select(x => x.sku).ToList();
            var duplicatedSkus = skus.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToHashSet();
            var existingSkus = (await _productVariantRepository.GetExistingSkusAsync(skus, cancellationToken)).ToHashSet();

            var allValueIds = items.SelectMany(i => i.option_values.Select(o => o.option_value_id)).Distinct().ToList();
            var valueMap = (await _optionValueRepository.OptionValueGetByIdsAsync(allValueIds, cancellationToken))
                .ToDictionary(v => v.id);

            var existingVariants = await _productVariantRepository
                .ProductVariantGetByProductIdWithOptionsAsync(product.id, cancellationToken);
            var seenCombos = existingVariants
                .Select(v => v.option_values.Select(x => x.option_value_id).ToHashSet())
                .ToList();

            string? Validate(CreateBatchProductVariantItem item, string sku)
            {
                if (string.IsNullOrWhiteSpace(sku)) return "SKU không được để trống";
                if (string.IsNullOrWhiteSpace(item.name)) return "Tên không được để trống";
                if (item.cost_price < 0 || item.sale_price < 0) return "Giá không hợp lệ";
                if (item.initial_quantity < 0 || item.initial_quantity > 1_000_000) return "Tồn đầu kỳ không hợp lệ";
                if (item.reorder_level < 0) return "Mức đặt hàng lại không hợp lệ";
                if (duplicatedSkus.Contains(sku)) return "SKU bị trùng trong danh sách gửi lên";
                if (existingSkus.Contains(sku)) return "SKU đã tồn tại";

                var defIds = item.option_values.Select(o => o.option_definition_id).ToList();
                if (defIds.Distinct().Count() != defIds.Count) return "option_definition_id bị trùng";
                if (allowedDefIds.Except(defIds).Any()) return "Thiếu giá trị cho option của danh mục";
                if (defIds.Except(allowedDefIds).Any()) return "Có option không thuộc danh mục";

                foreach (var o in item.option_values)
                {
                    if (!valueMap.TryGetValue(o.option_value_id, out var val))
                        return $"option_value_id {o.option_value_id} không tồn tại";
                    if (val.option_definition_id != o.option_definition_id)
                        return $"option_value_id {o.option_value_id} không thuộc option_definition_id {o.option_definition_id}";
                }
                return null;
            }

            var rows = new List<(ProductVariant? variant, CreateBatchProductVariantItem item, CreateBatchProductVariantResponseDTO dto)>();
            foreach (var (item, sku) in normalized)
            {
                var error = Validate(item, sku);

                if (error is null)
                {
                    var combo = item.option_values.Select(o => o.option_value_id).ToHashSet();
                    if (seenCombos.Any(c => c.SetEquals(combo)))
                        error = "Tổ hợp option bị trùng";
                    else
                        seenCombos.Add(combo);   
                }

                if (error is not null)
                {
                    rows.Add((null, item, CreateBatchProductVariantResponseDTO.Fail(sku, item.name, error)));
                    continue;
                }

                var variant = ProductVariant.Create(product.id, sku, item.name, item.cost_price,
                    item.sale_price, item.value_json, item.is_active);
                rows.Add((variant, item, CreateBatchProductVariantResponseDTO.Ok(sku, item.name)));
            }

            var valid = rows.Where(r => r.variant is not null).ToList();
            if (valid.Count > 0)
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    await _productVariantRepository.CreateBatchAsync(valid.Select(r => r.variant!).ToList(), cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);   // có id cho từng variant

                    foreach (var r in valid)
                        foreach (var o in r.item.option_values)
                            await _productVariantOptionValueRepository.CreateAsync(
                                ProductVariantOptionValue.Create(r.variant!.id, o.option_definition_id, o.option_value_id),
                                cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    foreach (var r in valid)
                        await CreateInventoryAsync(r.variant!, r.item.initial_quantity, r.item.reorder_level, cancellationToken);

                    await _unitOfWork.CommitTransactionAsync(cancellationToken);
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    throw;
                }
            }

            foreach (var r in rows)
                if (r.variant is not null) r.dto.id = r.variant.id;

            return OperationResult<List<CreateBatchProductVariantResponseDTO>>.Success(rows.Select(r => r.dto).ToList());
        }
        public async Task<OperationResult<ProductVariantDTO>> UpdateAsync(UpdateProductVariantRequestDTO model, CancellationToken cancellationToken = default)
        {
            ProductVariant product_variant = await _productVariantRepository.ProductVariantGetByIdAsync(model.id, cancellationToken);
            if (product_variant == null) throw new NotFoundException("Không tìm thấy danh mục.");

            if (await _productVariantRepository.ExistsBySkuAsync(model.sku, model.id, cancellationToken))
                throw new ConflictException("Sku đã tồn tại.");

            product_variant.Update(model.name, model.cost_price, model.sale_price, model.value_json, model.is_active);

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

        private async Task CreateInventoryAsync(ProductVariant variant, int initialQuantity, int reorderLevel, CancellationToken ct)
        {
            if (initialQuantity < 0 || initialQuantity > 1_000_000)
                throw new BadRequestException("Tồn đầu kỳ không hợp lệ.");

            var inventory = InventoryItem.Create(variant.id, reorderLevel);
            await _inventoryItemRepository.CreateAsync(inventory, ct);
            await _unitOfWork.SaveChangesAsync(ct);   

            if (initialQuantity > 0)
            {
                inventory.Increase(initialQuantity);
                await _inventoryTransactionRepository.CreateAsync(
                    InventoryTransaction.Create(inventory.id, InventoryTransactionType.Receive,
                        initialQuantity, "VariantCreate", variant.id, "Tồn đầu kỳ"), ct);
                await _unitOfWork.SaveChangesAsync(ct);
            }
        }
    }
}