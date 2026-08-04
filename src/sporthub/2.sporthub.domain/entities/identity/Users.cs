
namespace SportHub.Domain;

public class Users : AuditableEntity
{
    public string email { get; set; } = "";
    public string password_hash { get; set; } = "";
    public string name { get; set; } = "";
    public string phone { get; set; } = "";
    public string avatar_url { get; set; } = "";
    public UserStatus status { get; set; } = UserStatus.Active;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
}
