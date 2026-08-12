
namespace sporthub.domain;

public class Brand : AuditableEntity
{
    public string name { get; set; } = "";
    public string slug { get; set; } = "";
    public string? logo_url { get; set; }
    public string? description { get; set; }
    public bool is_active { get; set; } = true;

    public ICollection<Product> products { get; set; } = new List<Product>();
}
