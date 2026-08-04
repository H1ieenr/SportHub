using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportHub.Domain;

namespace SportHub.Repository.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<Users>
{
    public void Configure(EntityTypeBuilder<Users> builder)
    {
        builder.ToTable("users");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.password_hash).HasMaxLength(500).IsRequired();
        builder.Property(x => x.name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.phone).HasMaxLength(20).IsRequired();
        builder.Property(x => x.avatar_url).HasMaxLength(500);
        builder.Property(x => x.status).HasConversion<int>().IsRequired();

        builder.HasIndex(x => x.email).IsUnique();
        builder.HasIndex(x => x.phone).IsUnique();
    }
}

public class RolesConfiguration : IEntityTypeConfiguration<Roles>
{
    public void Configure(EntityTypeBuilder<Roles> builder)
    {
        builder.ToTable("roles");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.description).HasMaxLength(500);
        builder.HasIndex(x => x.name).IsUnique();
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles");
        builder.HasKey(x => new { x.user_id, x.role_id });

        builder.HasOne(x => x.user)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.user_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.role)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.role_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("addresses");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.receiverName).HasColumnName("receiver_name").HasMaxLength(200).IsRequired();
        builder.Property(x => x.receiverPhone).HasColumnName("receiver_phone").HasMaxLength(20).IsRequired();
        builder.Property(x => x.province).HasMaxLength(100).IsRequired();
        builder.Property(x => x.district).HasMaxLength(100).IsRequired();
        builder.Property(x => x.ward).HasMaxLength(100).IsRequired();
        builder.Property(x => x.addressLine).HasColumnName("address_line").HasMaxLength(500).IsRequired();

        builder.HasOne(x => x.user)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.user_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.user_id, x.is_default });
    }
}
