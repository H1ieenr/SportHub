
namespace sporthub.domain;

public class Address : AuditableEntity
{
    public long user_id { get; set; }
    public string receiverName { get; set; } = "";
    public string receiverPhone { get; set; } = "";
    public string province { get; set; } = "";
    public string district { get; set; } = "";
    public string ward { get; set; } = "";
    public string addressLine { get; set; } = "";
    public bool is_default { get; set; }

    public Users user { get; set; } = null!;
}
