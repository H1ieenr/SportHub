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
            // Nếu đã có Data rồi thì bỏ qua
            if (await context.Users.AnyAsync<Users>()) return;

            var defaultPasswordHash = passwordHasher.HashPassword(null!, "123");

            // Dùng Factory Method của Domain để khởi tạo
            var admin = Users.Create("admin@sporthub.com", defaultPasswordHash, "SportHub Admin", "0900000001");
            admin.AddRole(1); // Role Admin

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}