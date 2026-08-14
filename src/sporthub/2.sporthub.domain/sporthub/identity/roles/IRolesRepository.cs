using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sporthub.domain
{
    public interface IRolesRepository
    {
        Task<Roles?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}