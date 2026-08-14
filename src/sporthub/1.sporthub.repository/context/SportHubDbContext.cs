using System;
using Microsoft.EntityFrameworkCore;
using sporthub.domain;

namespace sporthub.repository;

public class SportHubDbContext(DbContextOptions<SportHubDbContext> options) : DbContext(options)
{
    public DbSet<Users> Users => Set<Users>();
    public DbSet<Roles> Roles => Set<Roles>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<RefreshTokens> RefreshTokens => Set<RefreshTokens>();

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SportHubDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        // #region seed data
        var seedDate = DateTime.Parse("2026-08-13T00:00:00+00:00");

        modelBuilder.Entity<Roles>().HasData(
            new Roles
            {
                id = 1,
                name = "Admin",
                description = "Quản trị toàn hệ thống",
                created_date = seedDate
            },
            new Roles
            {
                id = 2,
                name = "Staff",
                description = "Nhân viên vận hành",
                created_date = seedDate
            },
            new Roles
            {
                id = 3,
                name = "Customer",
                description = "Khách hàng",
                created_date = seedDate
            }
        );

        // modelBuilder.Entity<Users>().HasData(
        //     new Users
        //     {
        //         id = 1,
        //         email = "admin@sporthub.com",
        //         password_hash = "AQAAAAIAAYagAAAAECfqnlBxhia3TYVR3N9FWe7nFMGRjL55IIEj5FN5z58JyW6OsOBz5d3LDK+YQBIkYQ==",
        //         name = "SportHub Admin",
        //         phone = "0900000001",
        //         avatar_url = "",
        //         status = UserStatus.Active,
        //         created_date = seedDate
        //     },
        //     new Users
        //     {
        //         id = 2,
        //         email = "staff@sporthub.local",
        //         password_hash = "AQAAAAIAAYagAAAAECfqnlBxhia3TYVR3N9FWe7nFMGRjL55IIEj5FN5z58JyW6OsOBz5d3LDK+YQBIkYQ==",
        //         name = "SportHub Staff",
        //         phone = "0900000002",
        //         avatar_url = "",
        //         status = UserStatus.Active,
        //         created_date = seedDate
        //     },
        //     new Users
        //     {
        //         id = 3,
        //         email = "customer@sporthub.local",
        //         password_hash = "AQAAAAIAAYagAAAAECfqnlBxhia3TYVR3N9FWe7nFMGRjL55IIEj5FN5z58JyW6OsOBz5d3LDK+YQBIkYQ==",
        //         name = "SportHub Customer",
        //         phone = "0900000003",
        //         avatar_url = "",
        //         status = UserStatus.Active,
        //         created_date = seedDate
        //     }
        // );

        // modelBuilder.Entity<UserRole>().HasData(
        //     new UserRole { user_id = 1, role_id = 1 },
        //     new UserRole { user_id = 2, role_id = 2 },
        //     new UserRole { user_id = 3, role_id = 3 }
        // );
        // #endregion
    }
}
