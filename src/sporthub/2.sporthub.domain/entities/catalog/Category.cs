
namespace SportHub.Domain;

public class Category : AuditableEntity
{
    public string name { get; set; } = "";
    public string slug { get; set; } = "";
    public string? description { get; set; }
    public string? image_url { get; set; }
    public long? parent_id { get; set; }
    public int display_order { get; set; }
    public bool is_active { get; set; } = true;

    public Category? parent { get; set; }
    public ICollection<Category> children { get; set; } = new List<Category>();
    public ICollection<Product> products { get; set; } = new List<Product>();
}
