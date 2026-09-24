using Shared.Common;

namespace sporthub.app.contracts
{
    public interface IEnumHelperService
    {
       Task<OperationResult<List<EnumItemDTO>>> GetEnum(EnumRequestDTO model, CancellationToken cancellationToken = default);
    }
}