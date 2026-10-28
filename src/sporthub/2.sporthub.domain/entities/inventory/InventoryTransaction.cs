
using Shared.Exceptions;

namespace sporthub.domain;

public class InventoryTransaction : AuditableEntity
{
    public long inventory_item_id { get; private set; }
    public InventoryTransactionType type { get; private set; }
    public int quantity { get; private set; }
    public string? reference_type { get; private set; }
    public long? reference_id { get; private set; }
    public string? note { get; private set; }
    public long? created_by_user_id { get; private set; }

    public InventoryItem inventory_item { get; private set; } = null!;

    private InventoryTransaction() { }

    public static InventoryTransaction Create(long inventoryItemId, InventoryTransactionType type,
        int quantity, string? referenceType, long? referenceId, string? note)
    {
        if (inventoryItemId <= 0) throw new ValidationException("Tồn kho không hợp lệ.");
        if (quantity <= 0) throw new ValidationException("Số lượng phải lớn hơn 0.");

        return new InventoryTransaction
        {
            inventory_item_id = inventoryItemId,
            type = type,
            quantity = quantity,
            reference_type = referenceType,
            reference_id = referenceId,
            note = note
        };
    }
}
