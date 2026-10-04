using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository
{
    public class CategoryOptionRepository : SportHubGenericRepository<CategoryOption>, ICategoryOptionRepository
    {
        private readonly SportHubDbContext _context;
        public CategoryOptionRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<CategoryOption?> CategoryOptionGetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetByIdAsync(id, cancellationToken);
        }
        public Task<bool> ExistsByOptionColumnAsync(long categoryId, string optionColumn, CancellationToken cancellationToken = default)
        {
            return _context.CategoryOptions.AnyAsync(x => x.category_id == categoryId, cancellationToken);
        }
        public async Task<List<CategoryOption>> CategoryOptionGetNoPagingAsync(
                string? searchtext,
                long? categoryId,
                CancellationToken cancellationToken = default)
        {
            var query = _context.CategoryOptions.AsNoTracking().AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(x => x.category_id == categoryId);

            var items = await query.OrderBy(x => x.display_order).ToListAsync(cancellationToken);

            return items;
        }

        public Task CreateAsync(CategoryOption CategoryOption, CancellationToken cancellationToken = default)
        {
            return AddAsync(CategoryOption);
        }
        public Task CreateBatchAsync(List<CategoryOption> CategoryOptions, CancellationToken cancellationToken = default)
        {
            return AddRangeAsync(CategoryOptions);
        }
        public Task UpdateAsync(CategoryOption CategoryOption, CancellationToken cancellationToken = default)
        {
            Update(CategoryOption);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(CategoryOption CategoryOption, CancellationToken cancellationToken = default)
        {
            Delete(CategoryOption);
            return Task.CompletedTask;
        }
        public Task DeleteBatchAsync(List<CategoryOption> CategoryOptions, CancellationToken cancellationToken = default)
        {
            DeleteRange(CategoryOptions);
            return Task.CompletedTask;
        }
    }
}