namespace SportHub.Domain;

public class Roles : AuditableEntity
{
    public string name { get; set; } = "";
    public string? description { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
