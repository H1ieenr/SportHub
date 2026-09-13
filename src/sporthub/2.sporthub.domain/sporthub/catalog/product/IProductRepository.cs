

namespace sporthub.domain
{
    public interface IProductRepository
    {
        Task<Product?> ProductGetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySlugAsync(string slug, long? excludeId = null, CancellationToken cancellationToken = default);
        Task<List<Product>> GetFeatureAsync(CancellationToken cancellationToken = default);
        Task<(List<Product> items, int total)> ProductGetPagedAsync(
               int pageNumber,
               int pageSize,
               string? searchtext,
               string? sortBy,
               string? sortDir,
               bool? is_featured,
               ProductStatus? status,
               CancellationToken cancellationToken = default);
        Task<List<Product>> ProductGetNoPagingAsync(
               string? searchtext,
               bool? is_featured,
               ProductStatus? status,
               CancellationToken cancellationToken = default);
        Task CreateAsync(Product product, CancellationToken cancellationToken = default);
        Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
        Task DeleteAsync(Product product, CancellationToken cancellationToken = default);
    }
}