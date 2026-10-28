using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using sporthub.domain;

namespace sporthub.repository;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("inventory_items", t =>
        {
            t.HasCheckConstraint("CK_inventory_items_on_hand", "[quantity_on_hand] >= 0");
            t.HasCheckConstraint("CK_inventory_items_reserved", "[reserved_quantity] >= 0 AND [reserved_quantity] <= [quantity_on_hand]");
        });
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.row_version).IsRowVersion();

        builder.HasIndex(x => x.product_variant_id).IsUnique().HasFilter("[deleted_date] IS NULL");

        builder.HasOne(x => x.product_variant)
            .WithOne(x => x.inventory)
            .HasForeignKey<InventoryItem>(x => x.product_variant_id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.transactions)
            .WithOne(x => x.inventory_item)   
            .HasForeignKey(x => x.inventory_item_id)
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
