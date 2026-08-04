namespace SportHub.Domain;

public class Cart : AuditableEntity
{
    public long user_id { get; set; }
    public CartStatus status { get; set; } = CartStatus.Active;

    public Users user { get; set; } = null!;
    public ICollection<CartItem> items { get; set; } = new List<CartItem>();
}
