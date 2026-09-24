using Shared.Common;

namespace sporthub.app.contracts
{
    public interface IProductImageAppService
    {
        Task<OperationResult<ProductImageDTO>> CreateAsync(CreateProductImageRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<List<CreateBatchProductImageResponseDTO>>> CreateBatchAsync(CreateBatchProductImageRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> DeleteAsync(DeleteProductImageRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> UpdatePrimaryProductImageAsync(UpdatePrimaryProductImageRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> UpdateDisplayOrderProductImageAsync(UpdateDisplayOrderProductImageRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<List<ProductImageDTO>>> ProductImageGetNoPagingAsync(GetProductImageNoPagingRequestDTO model, CancellationToken cancellationToken = default);
    }
}