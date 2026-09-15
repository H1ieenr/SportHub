using Microsoft.EntityFrameworkCore;
using sporthub.domain;


namespace sporthub.repository
{
    public class ProductVariantRepository : SportHubGenericRepository<ProductVariant>, IProductVariantRepository
    {
        private readonly SportHubDbContext _context;
        public ProductVariantRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<ProductVariant?> ProductVariantGetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetByIdAsync(id, cancellationToken);
        }
        public Task<ProductVariant?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            return _context.ProductVariants.FirstOrDefaultAsync(x => x.sku == sku && x.deleted_date == null, cancellationToken);
        }
        public async Task<List<string>> GetExistingSkusAsync(List<string> skus, CancellationToken cancellationToken = default)
        {
            return await _context.ProductVariants
                .Where(x => x.deleted_date == null && skus.Contains(x.sku))
                .Select(x => x.sku)
                .ToListAsync(cancellationToken);
        }
        public Task<bool> ExistsBySkuAsync(string sku, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            return _context.ProductVariants.AnyAsync(x => x.sku == sku && (excludeId == null || x.id != excludeId), cancellationToken);
        }
        public Task<List<ProductVariant>> GetActiveByProductIdAsync(long product_id, CancellationToken cancellationToken = default)
        {
            return _context.ProductVariants.AsNoTracking().Where(x => x.is_active && x.product.id == product_id).OrderBy(x => x.name).ToListAsync(cancellationToken);
        }
        public async Task<(List<ProductVariant> items, int total)> ProductVariantGetPagedAsync(
                int pageNumber,
                int pageSize,
                string? searchtext,
                string? sortBy,
                string? sortDir,
                long? product_id,
                bool? is_active,
                CancellationToken cancellationToken = default)
        {
            var query = _context.ProductVariants.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchtext))
                query = query.Where(x => x.name.Contains(searchtext) || x.sku.Contains(searchtext));

            if (is_active.HasValue)
                query = query.Where(x => x.is_active == is_active.Value);

            if (product_id.HasValue)
                query = query.Where(x => x.product.id == product_id.Value);

            query = sortBy switch
            {
                "name" => sortDir == "desc" ? query.OrderByDescending(x => x.name) : query.OrderBy(x => x.name),
                "created_date" => sortDir == "desc" ? query.OrderByDescending(x => x.created_date) : query.OrderBy(x => x.created_date),
                _ => query.OrderByDescending(x => x.created_date)
            };

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).OrderBy(x => x.created_date)
                .ToListAsync(cancellationToken);

            return (items, total);
        }
        public async Task<List<ProductVariant>> ProductVariantGetNoPagingAsync(
              string? searchtext,
              long? product_id,
              bool? is_active,
              CancellationToken cancellationToken = default)
        {
            var query = _context.ProductVariants.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchtext))
                query = query.Where(x => x.name.Contains(searchtext) || x.sku.Contains(searchtext));

            if (is_active.HasValue)
                query = query.Where(x => x.is_active == is_active.Value);

            if (product_id.HasValue)
                query = query.Where(x => x.product.id == product_id.Value);


            var items = await query.OrderBy(x => x.created_date).ToListAsync(cancellationToken);

            return items;
        }
        public Task CreateAsync(ProductVariant productVariant, CancellationToken cancellationToken = default)
        {
            return AddAsync(productVariant);
        }
        public Task CreateBatchAsync(List<ProductVariant> productVariants, CancellationToken cancellationToken = default)
        {
            return AddRangeAsync(productVariants);
        }
        public Task UpdateAsync(ProductVariant productVariant, CancellationToken cancellationToken = default)
        {
            Update(productVariant);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(ProductVariant productVariant, CancellationToken cancellationToken = default)
        {
            Delete(productVariant);
            return Task.CompletedTask;
        }

    }
}