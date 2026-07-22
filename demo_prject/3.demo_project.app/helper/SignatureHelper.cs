using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace demo_project.app
{
    public static class SignatureHelper
    {
        public static string Generate(string formMiniAppId, string tenantId, string secret, long timestamp)
        {
            var raw = $"{formMiniAppId}:{tenantId}:{secret}:{timestamp}";

            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Chuyển đổi mỗi byte thành chuỗi hexadecimal
                }
                return builder.ToString();
            }
        }
    }

}
