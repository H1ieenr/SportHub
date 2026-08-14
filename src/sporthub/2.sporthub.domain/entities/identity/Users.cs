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
    public IReadOnlyCollection<RefreshTokens> refresh_tokens =>_refreshTokens;

    // Constructor
    private Users() { }

    // Static Factory Method
    public static Users Create(string email, string passwordHash, string name, string phone, string avatarUrl = "")
    {
        // if (string.IsNullOrWhiteSpace(email))
        //     throw new ValidationException("Email không được để trống.");

        // if (string.IsNullOrWhiteSpace(passwordHash))
        //     throw new ValidationException("Mật khẩu không được để trống.");

        return new Users
        {
            email = email.ToLower().Trim(),
            password_hash = passwordHash,
            name = name.Trim(),
            phone = phone?.Trim() ?? string.Empty,
            status = UserStatus.Active,
            avatar_url = avatarUrl
        };
    }

    // ────────── 5. CÁC DOMAIN METHODS (HÀNH VI NGHIỆP VỤ) ──────────

    /// <summary>
    /// Cập nhật thông tin cá nhân
    /// </summary>
    public void UpdateProfile(string name, string phone)
    {
        // if (string.IsNullOrWhiteSpace(name))
        //     throw new ValidationException("Tên người dùng không được để trống.");

        this.name = name.Trim();
        this.phone = phone?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Cập nhật ảnh đại diện
    /// </summary>
    public void UpdateAvatar(string avatarUrl)
    {
        // if (string.IsNullOrWhiteSpace(avatarUrl))
        //     throw new ValidationException("Đường dẫn ảnh đại diện không hợp lệ.");

        avatar_url = avatarUrl;
    }

    /// <summary>
    /// Thêm Role cho User (Tự rào chắn chống thêm trùng)
    /// </summary>
    public void AddRole(long roleId)
    {
        if (_userRoles.Any(r => r.role_id == roleId))
            return; 

        _userRoles.Add(new UserRole(id, roleId));
    }

    /// <summary>
    /// Thêm Refresh Token khi Login
    /// </summary>
    public void AddRefreshToken(string token, DateTime expiresAt)
    {
        _refreshTokens.Add(new RefreshTokens(id, token, expiresAt));
    }

    /// <summary>
    /// Đổi trạng thái tài khoản (Khóa/Kích hoạt)
    /// </summary>

    public void ChangeStatus(UserStatus newStatus)
    {
        status = newStatus;
    }
}
