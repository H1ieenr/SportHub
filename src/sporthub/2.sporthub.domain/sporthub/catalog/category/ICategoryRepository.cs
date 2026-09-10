
namespace sporthub.domain
{
    public interface ICategoryRepository
    {
        Task<Category?> CategoryGetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySlugAsync(string slug, long? excludeId = null, CancellationToken cancellationToken = default);
        Task<List<Category>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<(List<Category> items, int total)> CategoryGetPagedAsync(
         int pageNumber,
         int pageSize,
         string? searchtext,
         string? sortBy,
         string? sortDir,
         bool? active,
         long? parent_id,
         CancellationToken cancellationToken = default);
        Task<List<Category>> CategoryGetNoPagingAsync(
         string? searchtext,
         bool? active,
         long? parent_id,
         CancellationToken cancellationToken = default);
        Task CreateAsync(Category category, CancellationToken cancellationToken = default);
        Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
        Task DeleteAsync(Category category, CancellationToken cancellationToken = default);
    }
}