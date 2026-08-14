using Shared.Persistence;
using sporthub.domain;
namespace sporthub.repository
{
    public class SportHubUnitOfWork : UnitOfWork<SportHubDbContext>, ISportHubUnitOfWork
    {
        public SportHubUnitOfWork(SportHubDbContext context)
            : base(context)
        {
        }
        // public override Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        // {
        //     return base.SaveChangesAsync(cancellationToken);
        // }
    }
}