
namespace SportHub.Domain;

public class CartItem : AuditableEntity
{
    public long cart_id { get; set; }
    public long product_variant_id { get; set; }
    public int quantity { get; set; }
    public decimal unit_price { get; set; }

    public Cart cart { get; set; } = null!;
    public ProductVariant product_variant { get; set; } = null!;
}
