using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Common;
namespace sporthub.app.contracts
{
    public interface IAuthCommonAppService
    {
        Task<OperationResult<LoginResponseDTO>> RefreshTokenAsync(RefreshTokenRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> LogoutAsync(LogoutRequestDTO model, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> ChangePasswordAsync(ChangePasswordRequestDTO model, CancellationToken cancellationToken = default);
    }
}