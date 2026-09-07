using Shared.Common;

namespace sporthub.app.contracts
{
    public interface ICategoryAppService
    {
        #region admin
        Task<OperationResult<CategoryDTO>> CreateAsync(CreateCategoryRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<CategoryDTO>> UpdateAsync(UpdateCategoryRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> DeleteAsync(DeleteCategoryRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> UpdateActiveAsync(UpdateActiveCategoryRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<CategoryDTO>> CategoryGetByIdAsync(CategoryGetByIdRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<PagedResult<CategoryDTO>>> CategoryGetPagedAsync(GetCategoriesPagedRequestDTO model, CancellationToken cancellationToken = default);
        #endregion
    }
}