
namespace sporthub.domain
{
    public interface ICategoryOptionRepository
    {
        Task<CategoryOption?> CategoryOptionGetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<List<CategoryOption>> CategoryOptionGetNoPagingAsync(
                string? searchtext,
                long? categoryId,
                CancellationToken cancellationToken = default);
        Task<bool> ExistsByOptionColumnAsync(long categoryId, string optionColumn, CancellationToken cancellationToken = default);
        Task CreateAsync(CategoryOption categoryOption, CancellationToken cancellationToken = default);
        Task UpdateAsync(CategoryOption categoryOption, CancellationToken cancellationToken = default);
        Task DeleteAsync(CategoryOption categoryOption, CancellationToken cancellationToken = default);
        Task CreateBatchAsync(List<CategoryOption> categoryOptions, CancellationToken cancellationToken = default);
        Task DeleteBatchAsync(List<CategoryOption> categoryOptions, CancellationToken cancellationToken = default);
    }
}