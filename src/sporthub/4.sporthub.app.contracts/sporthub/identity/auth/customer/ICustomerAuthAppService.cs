using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Common;

namespace sporthub.app.contracts
{
    public interface ICustomerAuthAppService
    {
        Task<OperationResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<UsersDTO>> RegisterAsync(RegisterRequestDTO model, CancellationToken cancellationToken = default);
    }
}