
namespace sporthub.domain
{
    public interface IOptionDefinitionRepository
    {
        Task<OptionDefinition?> OptionDefinitionGetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<OptionDefinition?> OptionDefinitionGetByIdWithValuesAsync(long id, CancellationToken cancellationToken = default);
        Task<List<OptionDefinition>> OptionDefinitionGetNoPagingAsync(string searchText, string code, List<long>? ids, CancellationToken cancellationToken = default);
        Task<bool> OptionDefinitionExistsByCodeAsync(string code, long? excludeId = null, CancellationToken cancellationToken = default);
        Task<List<long>> OptionDefinitionExistingIdsAsync(List<long> ids, CancellationToken cancellationToken = default);
        Task CreateAsync(OptionDefinition entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(OptionDefinition entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(OptionDefinition entity, CancellationToken cancellationToken = default);
    }
}