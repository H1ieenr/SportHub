
namespace sporthub.domain
{
    public interface IBrandRepository
    {
        Task<Brand?> BrandGetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Brand?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySlugAsync(string slug, long? excludeId = null, CancellationToken cancellationToken = default);
        Task<List<Brand>> GetActiveAsync(CancellationToken cancellationToken = default);
        Task<(List<Brand> items, int total)> BrandGetPagedAsync(
         int pageNumber,
         int pageSize,
         string? searchtext,
         string? sortBy,
         string? sortDir,
         CancellationToken cancellationToken = default);
        Task CreateAsync(Brand brand, CancellationToken cancellationToken = default);
        Task UpdateAsync(Brand brand, CancellationToken cancellationToken = default);
        Task DeleteAsync(Brand brand, CancellationToken cancellationToken = default);
    }
}