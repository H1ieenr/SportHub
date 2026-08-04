using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SportHub.Domain;

namespace SportHub.Repository.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.slug).HasMaxLength(220).IsRequired();
        builder.Property(x => x.description).HasMaxLength(1000);
        builder.Property(x => x.image_url).HasMaxLength(500);
        builder.HasIndex(x => x.slug).IsUnique();

        builder.HasOne(x => x.parent)
            .WithMany(x => x.children)
            .HasForeignKey(x => x.parent_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("brands");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.name).HasMaxLength(150).IsRequired();
        builder.Property(x => x.slug).HasMaxLength(170).IsRequired();
        builder.Property(x => x.logo_url).HasMaxLength(500);
        builder.Property(x => x.description).HasMaxLength(1000);
        builder.HasIndex(x => x.slug).IsUnique();
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.name).HasMaxLength(250).IsRequired();
        builder.Property(x => x.slug).HasMaxLength(270).IsRequired();
        builder.Property(x => x.description).HasColumnType("nvarchar(max)");
        builder.Property(x => x.base_price).HasPrecision(18, 2);
        builder.Property(x => x.status).HasConversion<int>().IsRequired();
        builder.HasIndex(x => x.slug).IsUnique();
        builder.HasIndex(x => new { x.category_id, x.status });

        builder.HasOne(x => x.category)
            .WithMany(x => x.products)
            .HasForeignKey(x => x.category_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.brand)
            .WithMany(x => x.products)
            .HasForeignKey(x => x.brand_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    private static readonly ValueConverter<JsonElement?, string?> JsonElementConverter = new(
        value => value.HasValue ? value.Value.GetRawText() : null,
        value => string.IsNullOrWhiteSpace(value) ? null : JsonDocument.Parse(value).RootElement.Clone());

    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("product_variants", table =>
            table.HasCheckConstraint("CK_product_variants_value_json", "[value_json] IS NULL OR ISJSON([value_json]) = 1"));
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.sku).HasMaxLength(100).IsRequired();
        builder.Property(x => x.name).HasMaxLength(250).IsRequired();
        builder.Property(x => x.cost_price).HasPrecision(18, 2);
        builder.Property(x => x.sale_price).HasPrecision(18, 2);
        builder.Property(x => x.value_json)
            .HasColumnType("nvarchar(max)")
            .HasConversion(JsonElementConverter);
        builder.HasIndex(x => x.sku).IsUnique();
        builder.HasIndex(x => new { x.product_id, x.is_active });

        builder.HasOne(x => x.product)
            .WithMany(x => x.variants)
            .HasForeignKey(x => x.product_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("product_images");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.image_url).HasMaxLength(500).IsRequired();
        builder.HasIndex(x => new { x.product_id, x.display_order });

        builder.HasOne(x => x.product)
            .WithMany(x => x.images)
            .HasForeignKey(x => x.product_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.product_variant)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.product_variant_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
