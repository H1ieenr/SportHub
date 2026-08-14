namespace sporthub.domain;

public class Roles : AuditableEntity
{
    public string name { get; set; } = "";
    public string? description { get; set; }

    public ICollection<UserRole> user_roles { get; set; } = new List<UserRole>();
}
