using System.Text.Json;

namespace sporthub.domain;

public class ProductVariant : AuditableEntity
{
    public long product_id { get; set; }
    public string sku { get; set; } = "";
    public string name { get; set; } = "";
    public decimal cost_price { get; set; }
    public decimal sale_price { get; set; }
    public bool is_active { get; set; } = true;
    public JsonElement? value_json { get; set; } = null;
    public Product product { get; set; } = null!;
    public InventoryItem? inventory { get; set; }
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
}
