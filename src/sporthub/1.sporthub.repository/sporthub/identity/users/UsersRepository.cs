using Microsoft.EntityFrameworkCore;
using Shared.Persistence;
using sporthub.domain;

namespace sporthub.repository
{
    public class UsersRepository : SportHubGenericRepository<Users>, IUsersRepository
    {
        private readonly SportHubDbContext _context;
        public UsersRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Users?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return _context.Users.Include(x => x.user_roles).ThenInclude(x => x.roles).FirstOrDefaultAsync(x => x.id == id, cancellationToken);
        }
        public Task<Users?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return _context.Users.Include(x => x.user_roles).ThenInclude(x => x.roles).FirstOrDefaultAsync(x => x.email == email, cancellationToken);
        }
        public Task<bool> ExistsByEmailAsync(string email,CancellationToken cancellationToken = default)
        {
            return _context.Users.AnyAsync(x => x.email == email,cancellationToken);
        }
        public Task<bool> ExistsByPhoneAsync(string phone,CancellationToken cancellationToken = default)
        {
            return _context.Users.AnyAsync(x => x.phone == phone,cancellationToken);
        }
        public Task CreateAsync(Users user, CancellationToken cancellationToken = default)
        {
            return _context.Users.AddAsync(user, cancellationToken).AsTask();
        }
    }
}