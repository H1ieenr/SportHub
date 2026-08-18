using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Common;

namespace sporthub.app.contracts
{
    public interface IUsersAppService
    {
        Task<OperationResult<GetByIdAsyncDTO>> GetByIdAsync(GetByIdAsyncRquestDTO model, CancellationToken cancellationToken = default);
    }
}