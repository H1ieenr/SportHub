using System;

namespace sporthub.domain;

public class UserRole
{
    public long user_id { get; private set; }
    public long role_id { get; private set; }

    public Users users { get; private set; } = null!;
    public Roles roles { get; private set; } = null!;

    private UserRole() { } 
    #region Static Factory Method
    public UserRole(long userId, long roleId)
    {
        user_id = userId;
        role_id = roleId;
    }
    #endregion
}
