namespace SportHub.Domain;

public class UserRole
{
    public long user_id { get; set; }
    public long role_id { get; set; }

    public Users user { get; set; } = null!;
    public Roles role { get; set; } = null!;
}
