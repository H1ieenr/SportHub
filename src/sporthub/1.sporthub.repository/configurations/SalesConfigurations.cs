using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportHub.Domain;

namespace SportHub.Repository.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("carts");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.status).HasConversion<int>().IsRequired();
        builder.HasIndex(x => new { x.user_id, x.status });

        builder.HasOne(x => x.user)
            .WithMany()
            .HasForeignKey(x => x.user_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("cart_items");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.unit_price).HasPrecision(18, 2);
        builder.HasIndex(x => new { x.cart_id, x.product_variant_id }).IsUnique();

        builder.HasOne(x => x.cart)
            .WithMany(x => x.items)
            .HasForeignKey(x => x.cart_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.product_variant)
            .WithMany()
            .HasForeignKey(x => x.product_variant_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("orders");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.order_code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.customer_name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.customer_phone).HasMaxLength(20).IsRequired();
        builder.Property(x => x.shipping_address).HasMaxLength(1000);
        builder.Property(x => x.note).HasMaxLength(1000);
        builder.Property(x => x.sub_total).HasPrecision(18, 2);
        builder.Property(x => x.discount_amount).HasPrecision(18, 2);
        builder.Property(x => x.shipping_fee).HasPrecision(18, 2);
        builder.Property(x => x.total_amount).HasPrecision(18, 2);
        builder.Property(x => x.status).HasConversion<int>().IsRequired();
        builder.Property(x => x.payment_status).HasConversion<int>().IsRequired();
        builder.Property(x => x.payment_method).HasConversion<int>().IsRequired();
        builder.HasIndex(x => x.order_code).IsUnique();
        builder.HasIndex(x => new { x.user_id, x.created_date });
        builder.HasIndex(x => new { x.status, x.created_date });

        builder.HasOne(x => x.user)
            .WithMany()
            .HasForeignKey(x => x.user_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.product_name).HasMaxLength(250).IsRequired();
        builder.Property(x => x.variant_name).HasMaxLength(250).IsRequired();
        builder.Property(x => x.sku).HasMaxLength(100).IsRequired();
        builder.Property(x => x.unit_price).HasPrecision(18, 2);
        builder.Property(x => x.discount_amount).HasPrecision(18, 2);
        builder.Property(x => x.line_total).HasPrecision(18, 2);
        builder.HasIndex(x => x.order_id);

        builder.HasOne(x => x.order)
            .WithMany(x => x.items)
            .HasForeignKey(x => x.order_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.product_variant)
            .WithMany()
            .HasForeignKey(x => x.product_variant_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.method).HasConversion<int>().IsRequired();
        builder.Property(x => x.status).HasConversion<int>().IsRequired();
        builder.Property(x => x.amount).HasPrecision(18, 2);
        builder.Property(x => x.transaction_code).HasMaxLength(200);
        builder.Property(x => x.raw_response).HasColumnType("nvarchar(max)");
        builder.HasIndex(x => x.transaction_code)
            .IsUnique()
            .HasFilter("[transaction_code] IS NOT NULL");

        builder.HasOne(x => x.order)
            .WithMany(x => x.payments)
            .HasForeignKey(x => x.order_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
