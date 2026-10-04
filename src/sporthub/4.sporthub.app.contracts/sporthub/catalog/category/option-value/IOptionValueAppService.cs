using Shared.Common;

namespace sporthub.app.contracts
{
    public interface IOptionValueAppService
    {
        Task<OperationResult<List<CreateOptionValueResponseDTO>>> CreateValueAsync(CreateOptionValueRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<OptionValueDTO>> UpdateValueAsync(UpdateOptionValueRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> DeleteValueAsync(DeleteOptionValueRequestDTO model, CancellationToken cancellationToken = default);
    }
}