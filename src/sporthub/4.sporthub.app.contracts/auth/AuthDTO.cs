using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sporthub.app.contracts
{
    #region Register
    public class RegisterRequestDTO
    {
        public string email { get; set; } = "";
        public string password { get; set; } = "";
        public string name { get; set; } = "";
        public string phone { get; set; } = "";
    };
    #endregion

    #region Login
    public class LoginRequestDTO
    {
        public string email { get; set; } = "";
        public string password { get; set; } = "";
    };
    public class LoginResponseDTO
    {
        public string access_token { get; set; } = "";
        public string refresh_token { get; set; } = "";
        public DateTimeOffset access_token_expires_at { get; set; }
        public UserProfileDTO user { get; set; } = new();
    };
    #endregion

    #region User
    public class UserProfileDTO
    {
        public long id { get; set; }
        public string email { get; set; } = "";
        public string name { get; set; } = "";
        public string phone { get; set; } = "";
        public string avatar_url { get; set; } = "";
        public UserStatusDTO status {get; set;}
    };
    #endregion
}