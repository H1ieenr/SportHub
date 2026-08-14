using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Common;

namespace sporthub.app.contracts
{
    public interface IAuthAppService
    {
        Task<OperationResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO model, CancellationToken cancellationToken = default);
        // Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        // Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        // Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
        // Task LogoutAsync(RefreshTokenRequestDto request);
        // Task<UserProfileDto> GetProfileAsync(long userId);
    }
}