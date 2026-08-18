using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sporthub.api
{
    public static class AppPolicies
    {
        public const string Admin = "Admin";
        public const string Staff = "Staff";
        public const string Customer = "Customer";

        public const string AdminAndStaff = $"{Admin},{Staff}";
    }
}