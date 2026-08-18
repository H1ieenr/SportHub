

namespace sporthub.app
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = "";
        public string CustomerAudience { get; set; } 
        public string AdminAudience { get; set; }
        public string SecretKey { get; set; } = "";
        public int AccessTokenMinutes { get; set; } = 60;
        public int RefreshTokenDays { get; set; } = 15;
    }
}