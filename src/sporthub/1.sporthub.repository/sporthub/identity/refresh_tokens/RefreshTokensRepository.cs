using System;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence;
using sporthub.domain;

namespace sporthub.repository
{
    public class RefreshTokensRepository : SportHubGenericRepository<RefreshTokens>, IRefreshTokensRepository
    {
        private readonly SportHubDbContext _context;
        public RefreshTokensRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }
        public Task AddAsync(RefreshTokens refreshToken, CancellationToken cancellationToken = default)
        {
            return _context.RefreshTokens.AddAsync(refreshToken, cancellationToken).AsTask();
        }

        public Task<RefreshTokens?> GetActiveByTokenHashAsync(string tokenHash, CancellationToken cancellationToken = default)
        {
            var now = DateTime.Now;

            return _context.RefreshTokens.Include(x => x.user)
                                         .ThenInclude(x => x.user_roles)
                                         .ThenInclude(x => x.roles)
                                         .FirstOrDefaultAsync(x => x.token_hash == tokenHash
                                                                 && x.revoked_at == null
                                                                 && x.expires_at > now, cancellationToken);
        }

        public Task RevokeAllActiveByUserIdAsync(long userId, DateTime revokedAt, CancellationToken cancellationToken = default)
        {
            return _context.RefreshTokens.Where(x => x.user_id == userId && x.revoked_at == null).ExecuteUpdateAsync(
                    updates => updates.SetProperty(x => x.revoked_at, revokedAt), cancellationToken);
        }
    }
}