using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sporthub.domain
{
    public interface IRefreshTokensRepository
    {
        Task AddAsync(RefreshTokens refreshToken, CancellationToken cancellationToken = default);
        Task<RefreshTokens?> GetActiveByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task RevokeAllActiveByUserIdAsync(long userId, DateTime revokedAt, CancellationToken cancellationToken = default);
    }
}