using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sporthub.domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace sporthub.repository
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(SportHubDbContext context, IPasswordHasher<Users> passwordHasher)
        {
            if (await context.Users.AnyAsync<Users>()) return;
            if (await context.Roles.AnyAsync<Roles>()) return;

            var roles = new List<Roles>
        {
            Roles.Create("Admin", "Quản trị toàn hệ thống"),
            Roles.Create("Staff", "Nhân viên vận hành"),
            Roles.Create("Customer", "Khách hàng")
        };

            var defaultPasswordHash = passwordHasher.HashPassword(null!, "123");

            var admin = Users.Create("admin@sporthub.com", defaultPasswordHash, "SportHub Admin", "0900000001");
            admin.AddRole(1); // Role Admin

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}