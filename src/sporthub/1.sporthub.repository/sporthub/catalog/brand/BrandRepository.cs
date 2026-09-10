using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository
{
    public class BrandRepository : SportHubGenericRepository<Brand>, IBrandRepository
    {
        private readonly SportHubDbContext _context;
        public BrandRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<Brand?> BrandGetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetByIdAsync(id, cancellationToken);
        }
        public Task<Brand?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return _context.Brands.FirstOrDefaultAsync(x => x.slug == slug, cancellationToken);
        }
        public Task<bool> ExistsBySlugAsync(string slug, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            return _context.Brands.AnyAsync(x => x.slug == slug && (excludeId == null || x.id != excludeId), cancellationToken);
        }
        public Task<List<Brand>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return _context.Brands.AsNoTracking().Where(x => x.is_active).OrderBy(x => x.name).ToListAsync(cancellationToken);
        }
        public async Task<(List<Brand> items, int total)> BrandGetPagedAsync(
                int pageNumber,
                int pageSize,
                string? searchtext,
                string? sortBy,
                string? sortDir,
                bool? active,
                CancellationToken cancellationToken = default)
        {
            var query = _context.Brands.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchtext))
                query = query.Where(x => x.name.Contains(searchtext) || x.slug.Contains(searchtext));

            if(active.HasValue)
                query = query.Where(x => x.is_active == active.Value);

            query = sortBy switch
            {
                "name" => sortDir == "desc" ? query.OrderByDescending(x => x.name) : query.OrderBy(x => x.name),
                "created_date" => sortDir == "desc" ? query.OrderByDescending(x => x.created_date) : query.OrderBy(x => x.created_date),
                _ => query.OrderByDescending(x => x.created_date)
            };

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, total);
        }
        public Task CreateAsync(Brand brand, CancellationToken cancellationToken = default)
        {
            return AddAsync(brand);
        }
        public Task UpdateAsync(Brand brand, CancellationToken cancellationToken = default)
        {
            Update(brand);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(Brand brand, CancellationToken cancellationToken = default)
        {
            Delete(brand);
            return Task.CompletedTask;
        }
    }
}