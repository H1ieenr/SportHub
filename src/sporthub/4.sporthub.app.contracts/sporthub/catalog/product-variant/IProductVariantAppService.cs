using Shared.Common;

namespace sporthub.app.contracts
{
    public interface IProductVariantAppService
    {
        Task<OperationResult<ProductVariantDTO>> CreateAsync(CreateProductVariantRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<List<CreateBatchProductVariantResponseDTO>>> CreateBatchAsync(CreateBatchProductVariantRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<ProductVariantDTO>> UpdateAsync(UpdateProductVariantRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> DeleteAsync(DeleteProductVariantRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> UpdateActiveProductVariantAsync(UpdateActiveProductVariantRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<ProductVariantDTO>> ProductVariantGetByIdAsync(ProductVariantGetByIdRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<PagedResult<ProductVariantDTO>>> ProductVariantGetPagedAsync(GetProductVariantPagedRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<List<ProductVariantDTO>>> ProductVariantGetNoPagingAsync(GetProductVariantNoPagingRequestDTO model, CancellationToken cancellationToken = default);
    }
}