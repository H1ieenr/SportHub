using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sporthub.domain
{
    public interface IOptionValueRepository
    {
        Task<OptionValue?> OptionValueGetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<List<OptionValue>> OptionValueGetByDefinitionIdAsync(long optionDefinitionId, CancellationToken cancellationToken = default);
        Task<bool> OptionValueExistsAsync(long optionDefinitionId, string value, long? excludeId = null, CancellationToken cancellationToken = default);
        Task<List<string>> OptionValuesExistsAsync(long optionDefinitionId, List<string> values, CancellationToken cancellationToken = default);
        Task<bool> OptionValueIsUsedByAnyVariantAsync(long id, CancellationToken cancellationToken = default);
        Task CreateAsync(OptionValue entity, CancellationToken cancellationToken = default);
        Task CreateBatchAsync(List<OptionValue> entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(OptionValue entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(OptionValue entity, CancellationToken cancellationToken = default);
    }
}