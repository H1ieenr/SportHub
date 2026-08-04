using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportHub.Domain;

namespace SportHub.Repository.Configurations;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("inventory_items");
        builder.ConfigureAuditableEntity();

        builder.HasIndex(x => x.product_variant_id).IsUnique();

        builder.HasOne(x => x.product_variant)
            .WithOne(x => x.inventory)
            .HasForeignKey<InventoryItem>(x => x.product_variant_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.ToTable("inventory_transactions");
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.type).HasConversion<int>().IsRequired();
        builder.Property(x => x.reference_type).HasMaxLength(100);
        builder.Property(x => x.note).HasMaxLength(1000);
        builder.HasIndex(x => new { x.inventory_item_id, x.created_date });
        builder.HasIndex(x => new { x.reference_type, x.reference_id });

        builder.HasOne(x => x.inventory_item)
            .WithMany(x => x.transactions)
            .HasForeignKey(x => x.inventory_item_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Users>()
            .WithMany()
            .HasForeignKey(x => x.created_by_user_id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
