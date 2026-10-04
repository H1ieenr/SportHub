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
            return _context.OptionDefinitions.Include(x => x.values).FirstOrDefaultAsync(x => x.id == id, cancellationToken);
        }
        public Task<List<OptionDefinition>> OptionDefinitionGetNoPagingAsync(CancellationToken cancellationToken = default)
        {
            return _context.OptionDefinitions.Include(x => x.values).OrderBy(x => x.name).ToListAsync(cancellationToken);
        }
        public Task<bool> OptionDefinitionExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            return _context.OptionDefinitions.AnyAsync(x => x.code == code && (excludeId == null || x.id != excludeId), cancellationToken);
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