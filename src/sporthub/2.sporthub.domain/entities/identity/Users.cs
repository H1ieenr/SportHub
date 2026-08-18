using System;
using Shared.Exceptions;

namespace sporthub.domain;

public class Users : AuditableEntity
{
    public string email { get; private set; }
    public string password_hash { get; private set; }
    public string name { get; private set; }
    public string phone { get; private set; }
    public string avatar_url { get; private set; }
    public UserStatus status { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> user_roles => _userRoles;
    private readonly List<Address> _addresses = new();
    public IReadOnlyCollection<Address> addresses => _addresses;
    private readonly List<RefreshTokens> _refreshTokens = new();
    public IReadOnlyCollection<RefreshTokens> refresh_tokens => _refreshTokens;

    private Users() { }

    #region Static Factory Method
    public static Users Create(string email, string passwordHash, string name, string phone, 
            string avatarUrl = "", long user_id = 0)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("Email không được để trống.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ValidationException("Mật khẩu không được để trống.");

        return new Users
        {
            email = email.ToLower().Trim(),
            password_hash = passwordHash,
            name = name.Trim(),
            phone = phone?.Trim() ?? string.Empty,
            status = UserStatus.Active,
            avatar_url = avatarUrl,
            created_by = user_id,
        };
    }
    #region Update 
    public void Update(string name, string phone, string avatarUrl = "")
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Tên người dùng không được để trống.");

        this.name = name.Trim();
        this.phone = phone?.Trim() ?? string.Empty;
    }
    #endregion
    #region Update Avatar
    public void UpdateAvatar(string avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(avatarUrl))
            throw new ValidationException("Đường dẫn ảnh đại diện không hợp lệ.");
        avatar_url = avatarUrl;
    }
    #endregion
    #region Add Role
    public void AddRole(long roleId)
    {
        if (_userRoles.Any(r => r.role_id == roleId))
            return;

        _userRoles.Add(new UserRole(id, roleId));
    }
    #endregion
    #region Change Password
    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ValidationException("Mật khẩu mới không hợp lệ.");

        password_hash = newPasswordHash;
    }
    #endregion
    #region Change Status
    public void ChangeStatus(UserStatus newStatus)
    {
        status = newStatus;
    }
    #endregion
    #endregion
}
