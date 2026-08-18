using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Common;
namespace sporthub.app.contracts
{
    #region Register
    public class RegisterRequestDTO : BaseRequestDTO
    {
        public string email { get; set; } = "";
        public string password { get; set; } = "";
        public string name { get; set; } = "";
        public string phone { get; set; } = "";
        public string avatar_url { get; set; } = "";
    };
    #endregion

    #region Login
    public class LoginRequestDTO : BaseRequestDTO
    {
        public string email { get; set; } = "";
        public string password { get; set; } = "";
    };
    public class LoginResponseDTO
    {
        public string access_token { get; set; } = "";
        public string refresh_token { get; set; } = "";
        public DateTime access_token_expires_at { get; set; }
        public UsersDTO user { get; set; } = new();
    };
    #endregion

    #region Refresh Token
    public class RefreshTokenRequestDTO : BaseRequestDTO
    {
        public string refresh_token { get; set; } = "";
    }
    #endregion
    #region Logout
    public class LogoutRequestDTO : BaseRequestDTO
    {
        public string refresh_token { get; set; } = "";
    }
    #endregion
    #region ChangePassword
    public class ChangePasswordRequestDTO : BaseRequestDTO
    {
        public string old_password { get; set; }
        public string new_password { get; set; }
    }
    #endregion
}