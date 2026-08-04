
namespace SportHub.Domain;

public class ProductImage : AuditableEntity
{
    public long product_id { get; set; }
    public long? product_variant_id { get; set; }
    public string image_url { get; set; } = "";
    public int display_order { get; set; }
    public bool is_primary { get; set; }

    public Product product { get; set; } = null!;
    public ProductVariant? product_variant { get; set; }
}
