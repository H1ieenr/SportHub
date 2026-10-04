using Microsoft.EntityFrameworkCore;
using sporthub.domain;


namespace sporthub.repository
{
    public class OptionValueRepository : SportHubGenericRepository<OptionValue>, IOptionValueRepository
    {
        private readonly SportHubDbContext _context;
        public OptionValueRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<OptionValue?> OptionValueGetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return GetByIdAsync(id, cancellationToken);
        }

        public Task<List<OptionValue>> OptionValueGetByDefinitionIdAsync(long optionDefinitionId, CancellationToken cancellationToken = default)
        {
            return _context.OptionValues.Where(x => x.option_definition_id == optionDefinitionId).OrderBy(x => x.display_order).ToListAsync(cancellationToken);
        }

        public Task<bool> OptionValueExistsAsync(long optionDefinitionId, string value, long? excludeId = null, CancellationToken cancellationToken = default)
        {
            return _context.OptionValues.AnyAsync(x => x.option_definition_id == optionDefinitionId && x.value == value && (excludeId == null || x.id != excludeId), cancellationToken);
        }
        public async Task<List<string>> OptionValuesExistsAsync(long optionDefinitionId, List<string> values, CancellationToken cancellationToken = default)
        {
            return await _context.OptionValues
                .Where(x => x.option_definition_id == optionDefinitionId && x.deleted_date == null && values.Contains(x.value))
                .Select(x => x.value)
                .ToListAsync(cancellationToken);
        }
        public Task<bool> OptionValueIsUsedByAnyVariantAsync(long id, CancellationToken cancellationToken = default)
        {
            return _context.ProductVariantOptionValues.AnyAsync(x => x.option_value_id == id, cancellationToken);
        }

        public Task CreateAsync(OptionValue entity, CancellationToken cancellationToken = default)
        {
            return AddAsync(entity);
        }
        public Task CreateBatchAsync(List<OptionValue> entity, CancellationToken cancellationToken = default)
        {
            return AddRangeAsync(entity);
        }
        public Task UpdateAsync(OptionValue entity, CancellationToken cancellationToken = default)
        {
            Update(entity);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(OptionValue entity, CancellationToken cancellationToken = default)
        {
            Delete(entity);
            return Task.CompletedTask;
        }
    }
}