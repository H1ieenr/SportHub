using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository
{
    public class OptionDefinitionRepository : SportHubGenericRepository<OptionDefinition>, IOptionDefinitionRepository
    {
        private readonly SportHubDbContext _context;
        public OptionDefinitionRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<OptionDefinition?> OptionDefinitionGetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetByIdAsync(id, cancellationToken);
        }
        public Task<OptionDefinition?> OptionDefinitionGetByIdWithValuesAsync(long id, CancellationToken cancellationToken = default)
        {
            return _context.OptionDefinitions.Include(x => x.values.Where(v => v.deleted_date == null)).FirstOrDefaultAsync(x => x.id == id, cancellationToken);
        }
        public Task<List<OptionDefinition>> OptionDefinitionGetNoPagingAsync(string searchText, string code, List<long>? ids, CancellationToken cancellationToken = default)
        {
            var query = _context.OptionDefinitions.Include(x => x.values.Where(v => v.deleted_date == null)).AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
                query = query.Where(x => x.name.Contains(searchText));

            if (!string.IsNullOrEmpty(code))
                query = query.Where(x => x.code == code);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.id));

            return query.OrderBy(x => x.name).ToListAsync(cancellationToken);
        }
        public Task<bool> OptionDefinitionExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            return _context.OptionDefinitions.AnyAsync(x => x.code == code && (excludeId == null || x.id != excludeId), cancellationToken);
        }
        public Task<List<long>> OptionDefinitionExistingIdsAsync(List<long> ids, CancellationToken cancellationToken = default)
        {
            return _context.OptionDefinitions.Where(x => ids.Contains(x.id)).Select(x => x.id).ToListAsync(cancellationToken);
        }
        public Task CreateAsync(OptionDefinition entity, CancellationToken cancellationToken = default)
        {
            return AddAsync(entity);
        }
        public Task UpdateAsync(OptionDefinition entity, CancellationToken cancellationToken = default)
        {
            Update(entity);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(OptionDefinition entity, CancellationToken cancellationToken = default)
        {
            Delete(entity);
            return Task.CompletedTask;
        }
    }
}