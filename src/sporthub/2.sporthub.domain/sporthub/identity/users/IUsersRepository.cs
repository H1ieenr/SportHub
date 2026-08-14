using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sporthub.domain
{
    public interface IUsersRepository
    {
        Task<Users?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        Task<Users?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByPhoneAsync(string phone, CancellationToken cancellationToken = default);
        Task CreateAsync(Users user, CancellationToken cancellationToken = default);
    }
}