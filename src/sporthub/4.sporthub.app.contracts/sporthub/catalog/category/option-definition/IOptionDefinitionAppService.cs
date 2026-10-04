using Shared.Common;

namespace sporthub.app.contracts
{
    public interface IOptionDefinitionAppService
    {
        Task<OperationResult<OptionDefinitionDTO>> CreateAsync(CreateOptionDefinitionRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<OptionDefinitionDTO>> UpdateAsync(UpdateOptionDefinitionRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> DeleteAsync(long id, CancellationToken cancellationToken = default);
        Task<OperationResult<List<OptionDefinitionDTO>>> GetNoPagingAsync(CancellationToken cancellationToken = default);
    }
}