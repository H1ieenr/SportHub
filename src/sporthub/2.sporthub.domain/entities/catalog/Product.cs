
namespace sporthub.domain;

public class Product : AuditableEntity
{
    public long category_id { get; set; }
    public long? brand_id { get; set; }
    public string name { get; set; } = "";
    public string slug { get; set; } = "";
    public string? description { get; set; }
    public decimal base_price { get; set; }
    public ProductStatus status { get; set; } = ProductStatus.Draft;
    public bool is_featured { get; set; }

    public Category category { get; set; } = null!;
    public Brand? brand { get; set; }
    public ICollection<ProductVariant> variants { get; set; } = new List<ProductVariant>();
    public ICollection<ProductImage> images { get; set; } = new List<ProductImage>();
}
