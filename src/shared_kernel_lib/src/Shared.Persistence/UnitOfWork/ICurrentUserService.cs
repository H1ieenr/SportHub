using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Persistence
{
    public interface ICurrentUserService
    {
        long? UserId { get; }
        bool IsAuthenticated { get; }
    }
}