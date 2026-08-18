
using System;

namespace sporthub.app.contracts
{
    #region CreateAccessToken
    public class CreateAccessTokenRequestDTO
    {
        public UsersDTO user { get; set; } = new UsersDTO();
        public string audience { get; set; }         
    }
    public class CreateAccessTokenResponseDTO
    {
        public string access_token { get; set; } = "";
        public DateTime expires_at { get; set; }
    }
    #endregion
    #region CreateRefreshToken
    public class CreateRefreshTokenResponseDTO
    {
        public string refresh_token { get; set; } = "";
    }
    #endregion
    #region HashRefreshToken
    public class HashRefreshTokenRequestDTO
    {
        public string rawToken { get; set; } = "";
    }
    public class HashRefreshTokenResponseDTO
    {
        public string hash_refresh_token { get; set; } = "";
    }
    #endregion
    #region GetRefreshTokenExpiry
    public class GetRefreshTokenExpiryResponseDTO
    {
        public DateTime refresh_token_expiry { get; set; } = new DateTime();
    }
    #endregion
}