
namespace sporthub.domain
{
    public interface IInventoryItemRepository
    {
        Task<InventoryItem?> InventoryItemGetByVariantIdAsync(long variantId, CancellationToken cancellationToken = default);         
        Task<InventoryItem?> InventoryItemGetTrackedByVariantIdAsync(long variantId, CancellationToken cancellationToken = default);   
        Task<(List<InventoryItem> items, int total)> InventoryItemGetPagedAsync(
            int pageNumber, int pageSize, string? searchText, long? producancellationTokenId, bool? lowStock,
            string? sortBy, string? sortDir, CancellationToken cancellationToken = default);
        Task CreateAsync(InventoryItem item, CancellationToken cancellationToken = default);
    }
}
