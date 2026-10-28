using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository
{
    public class InventoryTransactionRepository : SportHubGenericRepository<InventoryTransaction>, IInventoryTransactionRepository
    {
        private readonly SportHubDbContext _context;
        public InventoryTransactionRepository(SportHubDbContext context) : base(context) => _context = context;

        public Task CreateAsync(InventoryTransaction transaction, CancellationToken cancellationToken = default)
        {
            return AddAsync(transaction);
        }

        public async Task<(List<InventoryTransaction> items, int total)> InventoryTransactionGetPagedAsync(
            int pageNumber, int pageSize, long variantId, InventoryTransactionType? type,
            DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
        {
            var query = _context.InventoryTransactions.AsNoTracking()
                .Where(x => x.inventory_item.product_variant_id == variantId);

            if (type.HasValue) query = query.Where(x => x.type == type);
            if (fromDate.HasValue) query = query.Where(x => x.created_date >= fromDate);
            if (toDate.HasValue) query = query.Where(x => x.created_date < toDate.Value.Date.AddDays(1));

            var total = await query.CountAsync(cancellationToken);
            var items = await query.OrderByDescending(x => x.created_date).ThenByDescending(x => x.id)
                .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return (items, total);
        }
    }
}
