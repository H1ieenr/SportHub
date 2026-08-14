using Microsoft.EntityFrameworkCore;
using Shared.Persistence;
using sporthub.domain;

namespace sporthub.repository
{
    public class RolesRepository : SportHubGenericRepository<Roles>, IRolesRepository
    {
        private readonly SportHubDbContext _context;
        public RolesRepository(SportHubDbContext context) : base(context)
        {
            _context = context;
        }
        public Task<Roles?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return _context.Roles.FirstOrDefaultAsync(x => x.name == name, cancellationToken);
        }
    }
}