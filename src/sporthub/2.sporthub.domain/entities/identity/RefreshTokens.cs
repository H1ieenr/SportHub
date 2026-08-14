using System;

namespace sporthub.domain;

public class RefreshTokens : AuditableEntity
{
    public long user_id { get; private set; }
    public string token_hash { get; private set; }
    public DateTime expires_at { get; private set; }
    public DateTime? revoked_at { get; private set; }
    public long? replaced_by_token_id { get; private set; }
    public string? device_name { get; private set; }
    public string? ip_address { get; private set; }

    public Users user { get; set; } = null!;
    public RefreshTokens? replaced_by_token { get; set; }

    public RefreshTokens() { }

    public RefreshTokens(long userId, string tokenHash, DateTime expiresAt)
    {
        user_id = userId;
        token_hash = tokenHash;
        expires_at = expiresAt;
    }
}
