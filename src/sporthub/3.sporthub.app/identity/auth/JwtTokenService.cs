using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using sporthub.app.contracts;
using System.IO;
using sporthub.domain;
namespace sporthub.app
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;

        public JwtTokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }
        public CreateAccessTokenResponseDTO CreateAccessToken(CreateAccessTokenRequestDTO model)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_settings.AccessTokenMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, model.user.id.ToString()),
                new Claim(ClaimTypes.Email, model.user.email),
                new Claim(ClaimTypes.Name, model.user.name)
            };

            claims.AddRange(model.roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new CreateAccessTokenResponseDTO
            {
                access_token = new JwtSecurityTokenHandler().WriteToken(token),
                expires_at = expiresAt
            };
        }
        public CreateRefreshTokenResponseDTO CreateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return new CreateRefreshTokenResponseDTO
            {
                refresh_token = Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "")
            };
        }
        public HashRefreshTokenResponseDTO HashRefreshToken(HashRefreshTokenRequestDTO model)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(model.rawToken));
            return new HashRefreshTokenResponseDTO
            {
                hash_refresh_token = Convert.ToHexString(hash)
            };
        }
        public GetRefreshTokenExpiryResponseDTO GetRefreshTokenExpiry()
        {
            return new GetRefreshTokenExpiryResponseDTO
            {
                refresh_token_expiry = DateTime.UtcNow.AddDays(_settings.RefreshTokenDays)
            };
        }
    }
}