using Shared.Persistence;
using sporthub.domain;
namespace sporthub.repository
{
    public class SportHubUnitOfWork : UnitOfWork<SportHubDbContext>, ISportHubUnitOfWork
    {
        public SportHubUnitOfWork(SportHubDbContext context, ICurrentUserService currentUserService)
            : base(context, currentUserService)
        {
        }
    }
}