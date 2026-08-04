
namespace SportHub.Domain;

public class OrderItem : AuditableEntity
{
    public long order_id { get; set; }
    public long product_variant_id { get; set; }
    public string product_name { get; set; } = "";
    public string variant_name { get; set; } = "";
    public string sku { get; set; } = "";
    public decimal unit_price { get; set; }
    public int quantity { get; set; }
    public decimal discount_amount { get; set; }
    public decimal line_total { get; set; }

    public Order order { get; set; } = null!;
    public ProductVariant product_variant { get; set; } = null!;
}
