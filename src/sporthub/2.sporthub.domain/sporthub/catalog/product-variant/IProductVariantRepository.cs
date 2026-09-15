
namespace sporthub.domain
{
    public interface IProductVariantRepository
    {
        Task<ProductVariant?> ProductVariantGetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<ProductVariant?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySkuAsync(string sku, long? excludeId = null, CancellationToken cancellationToken = default);
        Task<List<string>> GetExistingSkusAsync(List<string> skus, CancellationToken cancellationToken = default);

        Task<List<ProductVariant>> GetActiveByProductIdAsync(long product_id, CancellationToken cancellationToken = default);
        Task<(List<ProductVariant> items, int total)> ProductVariantGetPagedAsync(
                int pageNumber,
                int pageSize,
                string? searchtext,
                string? sortBy,
                string? sortDir,
                long? product_id,
                bool? is_active,
                CancellationToken cancellationToken = default);
        Task<List<ProductVariant>> ProductVariantGetNoPagingAsync(
                string? searchtext,
                long? product_id,
                bool? is_active,
                CancellationToken cancellationToken = default);
        Task CreateAsync(ProductVariant productVariant, CancellationToken cancellationToken = default);
        Task CreateBatchAsync(List<ProductVariant> productVariants, CancellationToken cancellationToken = default);
        Task UpdateAsync(ProductVariant productVariant, CancellationToken cancellationToken = default);
        Task DeleteAsync(ProductVariant productVariant, CancellationToken cancellationToken = default);
    }
}