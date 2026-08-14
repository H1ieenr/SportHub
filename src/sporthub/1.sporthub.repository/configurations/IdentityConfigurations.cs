using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sporthub.domain;

namespace sporthub.repository;

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

        builder.HasOne(x => x.users)
            .WithMany(x => x.user_roles)
            .HasForeignKey(x => x.user_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.roles)
            .WithMany(x => x.user_roles)
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
            .WithMany(x => x.addresses)
            .HasForeignKey(x => x.user_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.user_id, x.is_default });
    }
}

public class RefreshTokensConfiguration : IEntityTypeConfiguration<RefreshTokens>
{
    public void Configure(EntityTypeBuilder<RefreshTokens> builder)
    {
        builder.ToTable("refresh_tokens");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.token_hash).HasMaxLength(128).IsRequired();
        builder.Property(x => x.device_name).HasMaxLength(200);
        builder.Property(x => x.ip_address).HasMaxLength(45);
        builder.HasIndex(x => x.token_hash).IsUnique();
        builder.HasIndex(x => new { x.user_id, x.expires_at });

        builder.HasOne(x => x.user)
            .WithMany(x => x.refresh_tokens)
            .HasForeignKey(x => x.user_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.replaced_by_token)
            .WithMany()
            .HasForeignKey(x => x.replaced_by_token_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
