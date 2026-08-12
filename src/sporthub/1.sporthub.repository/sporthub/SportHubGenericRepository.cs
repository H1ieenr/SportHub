using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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