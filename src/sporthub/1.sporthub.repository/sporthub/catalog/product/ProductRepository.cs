using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository
{
    public class ProductRepository : SportHubGenericRepository<Product>, IProductRepository
    {
        private readonly SportHubDbContext _context;
        public ProductRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Product?> ProductGetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetByIdAsync(id, cancellationToken);
        }
        public Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return _context.Products.FirstOrDefaultAsync(x => x.slug == slug, cancellationToken);
        }
        public Task<bool> ExistsBySlugAsync(string slug, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            return _context.Products.AnyAsync(x => x.slug == slug && (excludeId == null || x.id != excludeId), cancellationToken);
        }
        public Task<List<Product>> GetFeatureAsync(CancellationToken cancellationToken = default)
        {
            return _context.Products.AsNoTracking().Where(x => x.is_featured).OrderBy(x => x.name).ToListAsync(cancellationToken);
        }
        public async Task<(List<Product> items, int total)> ProductGetPagedAsync(
                int pageNumber,
                int pageSize,
                string? searchtext,
                string? sortBy,
                string? sortDir,
                bool? is_featured,
                ProductStatus? status,
                CancellationToken cancellationToken = default)
        {
            var query = _context.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchtext))
                query = query.Where(x => x.name.Contains(searchtext) || x.slug.Contains(searchtext));

            if (is_featured.HasValue)
                query = query.Where(x => x.is_featured == is_featured.Value);

            if (status.HasValue)
                query = query.Where(x => x.status == status.Value);

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
        public async Task<List<Product>> ProductGetNoPagingAsync(
              string? searchtext,
              bool? is_featured,
              ProductStatus? status,
              CancellationToken cancellationToken = default)
        {
            var query = _context.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchtext))
                query = query.Where(x => x.name.Contains(searchtext) || x.slug.Contains(searchtext));

            if (is_featured.HasValue)
                query = query.Where(x => x.is_featured == is_featured.Value);

            if (status.HasValue)
                query = query.Where(x => x.status == status.Value);


            var items = await query.OrderBy(x => x.created_date).ToListAsync(cancellationToken);

            return items;
        }
        public Task CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            return AddAsync(product);
        }
        public Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            Update(product);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
        {
            Delete(product);
            return Task.CompletedTask;
        }
    }
}