using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository
{
    public class InventoryItemRepository : SportHubGenericRepository<InventoryItem>, IInventoryItemRepository
    {
        private readonly SportHubDbContext _context;
        public InventoryItemRepository(SportHubDbContext context) : base(context) => _context = context;

        public Task<InventoryItem?> InventoryItemGetByVariantIdAsync(long variantId, CancellationToken cancellationToken = default)
        {
            return _context.InventoryItems.AsNoTracking().Include(x => x.product_variant).ThenInclude(v => v.product)
                                          .FirstOrDefaultAsync(x => x.product_variant_id == variantId, cancellationToken);
        }

        public Task<InventoryItem?> InventoryItemGetTrackedByVariantIdAsync(long variantId, CancellationToken cancellationToken = default)
        {
            return _context.InventoryItems.FirstOrDefaultAsync(x => x.product_variant_id == variantId, cancellationToken);
        }

        public async Task<(List<InventoryItem> items, int total)> InventoryItemGetPagedAsync(
            int pageNumber, int pageSize, string? searchText, long? productId, bool? lowStock,
            string? sortBy, string? sortDir, CancellationToken cancellationToken = default)
        {
            var query = _context.InventoryItems.AsNoTracking()
                .Include(x => x.product_variant).ThenInclude(v => v.product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
                query = query.Where(x => x.product_variant.sku.Contains(searchText)
                                      || x.product_variant.product.name.Contains(searchText));
            if (productId.HasValue)
                query = query.Where(x => x.product_variant.product_id == productId);
            if (lowStock == true)
                query = query.Where(x => x.quantity_on_hand - x.reserved_quantity <= x.reorder_level);

            var total = await query.CountAsync(cancellationToken);

            var desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
            query = sortBy?.ToLowerInvariant() switch
            {
                "sku" => desc ? query.OrderByDescending(x => x.product_variant.sku) : query.OrderBy(x => x.product_variant.sku),
                "on_hand" => desc ? query.OrderByDescending(x => x.quantity_on_hand) : query.OrderBy(x => x.quantity_on_hand),
                "available" => desc ? query.OrderByDescending(x => x.quantity_on_hand - x.reserved_quantity)
                                    : query.OrderBy(x => x.quantity_on_hand - x.reserved_quantity),
                _ => query.OrderByDescending(x => x.id)
            };

            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return (items, total);
        }

        public Task CreateAsync(InventoryItem item, CancellationToken cancellation = default)
        {
            return AddAsync(item);
        }
    }
}
