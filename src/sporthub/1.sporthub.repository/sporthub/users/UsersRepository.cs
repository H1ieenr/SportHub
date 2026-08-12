using Microsoft.EntityFrameworkCore;
using Shared.Persistence;
using sporthub.domain;

namespace sporthub.repository
{
    public class UsersRepository : SportHubGenericRepository<Users>, IUsersRepository
    {
        public UsersRepository(SportHubDbContext context) : base(context)
        {
        }
        
        public async Task<Users?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x =>
                x.email == email &&
                !x.is_deleted);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(x =>
                x.email == email &&
                !x.is_deleted);
        }
    }
}