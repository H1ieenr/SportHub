
namespace SportHub.Domain;

public class InventoryTransaction : AuditableEntity
{
    public long inventory_item_id { get; set; }
    public InventoryTransactionType type { get; set; }
    public int quantity { get; set; }
    public string? reference_type { get; set; }
    public long? reference_id { get; set; }
    public string? note { get; set; }
    public long? created_by_user_id { get; set; }

    public InventoryItem inventory_item { get; set; } = null!;
}
