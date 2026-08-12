
namespace sporthub.domain;

public class InventoryItem : AuditableEntity
{
    public long product_variant_id { get; set; }
    public int quantity_on_hand { get; set; }
    public int reserved_quantity { get; set; }
    public int reorder_level { get; set; }
    public int available_quantity => quantity_on_hand - reserved_quantity;

    public ProductVariant product_variant { get; set; } = null!;
    public ICollection<InventoryTransaction> transactions { get; set; } = new List<InventoryTransaction>();
}
