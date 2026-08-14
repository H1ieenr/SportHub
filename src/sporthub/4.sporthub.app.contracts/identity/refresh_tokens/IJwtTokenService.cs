using System;

namespace sporthub.app.contracts
{
    public interface IJwtTokenService
    {
        CreateAccessTokenResponseDTO CreateAccessToken(CreateAccessTokenRequestDTO model);
        CreateRefreshTokenResponseDTO CreateRefreshToken();
        HashRefreshTokenResponseDTO HashRefreshToken(HashRefreshTokenRequestDTO model);
        GetRefreshTokenExpiryResponseDTO GetRefreshTokenExpiry();
    }
}