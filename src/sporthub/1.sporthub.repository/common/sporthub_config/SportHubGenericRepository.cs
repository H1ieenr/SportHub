using Shared.Persistence;

namespace sporthub.repository
{
    public class SportHubGenericRepository<T>: GenericRepository<T, SportHubDbContext>
    where T : class
    {
        public SportHubGenericRepository(SportHubDbContext context)
            : base(context)
        {
        }
    }
}