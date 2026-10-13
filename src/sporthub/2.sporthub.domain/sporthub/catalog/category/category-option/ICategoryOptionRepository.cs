
namespace sporthub.domain
{
    public interface ICategoryOptionRepository
    {
        Task<CategoryOption?> CategoryOptionGetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<List<CategoryOption>> CategoryOptionGetNoPagingAsync(
            string? searchtext, long? categoryId, CancellationToken cancellationToken = default);
        Task<List<CategoryOption>> CategoryOptionGetTrackedByCategoryIdAsync(
            long categoryId, CancellationToken cancellationToken = default);
        Task CreateAsync(CategoryOption categoryOption, CancellationToken cancellationToken = default);
        Task UpdateAsync(CategoryOption categoryOption, CancellationToken cancellationToken = default);
        Task DeleteAsync(CategoryOption categoryOption, CancellationToken cancellationToken = default);

    }
}