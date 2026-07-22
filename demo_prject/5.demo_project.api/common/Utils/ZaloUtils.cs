using System;
using System.Security.Cryptography;
using System.Text;

namespace demo_project.api
{
    public class ZaloUtils
    {

        // Utils: PKCE
        public (string verifier, string challenge) CreatePkcePair()
        {
            // verifier: 43..128 URL-safe chars
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            var verifier = Base64UrlEncode(bytes); // ~43 chars

            // challenge = BASE64URL-ENCODE(SHA256(verifier))
            using var sha = SHA256.Create();
            var hashed = sha.ComputeHash(Encoding.ASCII.GetBytes(verifier));
            var challenge = Base64UrlEncode(hashed);
            return (verifier, challenge);

            static string Base64UrlEncode(byte[] input) =>
                Convert.ToBase64String(input)
                    .Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
    }
}
