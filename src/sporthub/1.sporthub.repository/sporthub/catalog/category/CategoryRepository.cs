using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository
{
    public class CategoryRepository : SportHubGenericRepository<Category>, ICategoryRepository
    {
        private readonly SportHubDbContext _context;
        public CategoryRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Category?> CategoryGetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetByIdAsync(id, cancellationToken);
        }
        public Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return _context.Categories.FirstOrDefaultAsync(x => x.slug == slug, cancellationToken);
        }
        public Task<bool> ExistsBySlugAsync(string slug, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            return _context.Categories.AnyAsync(x => x.slug == slug && (excludeId == null || x.id != excludeId), cancellationToken);
        }
        public Task<List<Category>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return _context.Categories.AsNoTracking().Where(x => x.is_active).OrderBy(x => x.name).ToListAsync(cancellationToken);
        }
        public async Task<(List<Category> items, int total)> CategoryGetPagedAsync(
                int pageNumber,
                int pageSize,
                string? searchtext,
                string? sortBy,
                string? sortDir,
                CancellationToken cancellationToken = default)
        {
            var query = _context.Categories.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchtext))
                query = query.Where(x => x.name.Contains(searchtext) || x.slug.Contains(searchtext));

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
        public Task CreateAsync(Category category, CancellationToken cancellationToken = default)
        {
            return AddAsync(category);
        }
        public Task UpdateAsync(Category category, CancellationToken cancellationToken = default)
        {
            Update(category);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(Category category, CancellationToken cancellationToken = default)
        {
            Delete(category);
            return Task.CompletedTask;
        }
    }
}