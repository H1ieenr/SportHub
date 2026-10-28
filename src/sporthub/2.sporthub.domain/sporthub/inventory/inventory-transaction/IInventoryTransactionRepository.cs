

namespace sporthub.domain
{ 
    public interface IInventoryTransactionRepository
    {
        Task CreateAsync(InventoryTransaction transaction, CancellationToken cancellationToken = default);
        Task<(List<InventoryTransaction> items, int total)> InventoryTransactionGetPagedAsync(
            int pageNumber, int pageSize, long variantId, InventoryTransactionType? type,
            DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
    }
}
