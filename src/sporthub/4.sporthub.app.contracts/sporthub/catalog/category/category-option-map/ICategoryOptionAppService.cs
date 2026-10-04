using Shared.Common;

namespace sporthub.app.contracts
{
    public interface ICategoryOptionAppService
    {
        Task<OperationResult<List<CategoryOptionDTO>>> CategoryOptionGetNoPagingAsync(CategoryOptionGetNoPagingRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<CategoryOptionDTO>> CategoryOptionGetByIdAsync(CategoryOptionGetByIdRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> ReplaceAllAsync(ReplaceAllAsyncRequestDTO model, CancellationToken cancellationToken = default);
    }
}