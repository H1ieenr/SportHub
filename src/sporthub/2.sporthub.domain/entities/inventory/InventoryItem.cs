
using Shared.Exceptions;

namespace sporthub.domain;

public class InventoryItem : AuditableEntity
{
    public long product_variant_id { get; private set; }
    public int quantity_on_hand { get; private set; }
    public int reserved_quantity { get; private set; }
    public int reorder_level { get; private set; }
    public byte[] row_version { get; private set; } = [];
    public int available_quantity => quantity_on_hand - reserved_quantity;

    public ProductVariant product_variant { get; private set; } = null!;
    private readonly List<InventoryTransaction> _transactions = new();
    public IReadOnlyCollection<InventoryTransaction> transactions => _transactions;

    private InventoryItem() { }

    public static InventoryItem Create(long productVariantId, int reorderLevel = 0)
    {
        if (productVariantId <= 0)
            throw new ValidationException("Biến thể không hợp lệ.");
        if (reorderLevel < 0)
            throw new ValidationException("Mức đặt hàng lại không hợp lệ.");

        return new InventoryItem
        {
            product_variant_id = productVariantId,
            reorder_level = reorderLevel
        };
    }

    public void Increase(int qty)
    {
        if (qty <= 0) throw new ValidationException("Số lượng phải lớn hơn 0.");
        quantity_on_hand += qty;
    }

    public void Decrease(int qty)
    {
        if (qty <= 0) throw new ValidationException("Số lượng phải lớn hơn 0.");
        if (qty > available_quantity) throw new ConflictException("Không đủ tồn kho khả dụng.");
        quantity_on_hand -= qty;
    }

    public void SetReorderLevel(int level)
    {
        if (level < 0) throw new ValidationException("Mức đặt hàng lại không hợp lệ.");
        reorder_level = level;
    }
}
