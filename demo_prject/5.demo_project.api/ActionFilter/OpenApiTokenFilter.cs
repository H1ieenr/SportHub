
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace demo_project.api.filter
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//    public class OpenApiAuthorizeAttribute : Attribute, IAuthorizationFilter
//    {
//        public string RequiredUserType { get; set; }

//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            var req = context.HttpContext.Request;
//            var authHeader = req.Headers["Authorization"].FirstOrDefault();

//            // 1) Missing Authorization header
//            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
//            {
//                Fail(context, 401, "ERR_MISSING_AUTH_HEADER", "Missing Authorization header");
//                return;
//            }

//            var token = authHeader.Substring("Bearer ".Length).Trim();

//            // 2) Parse body → tenantId
//            var tenantId = ReadTenantId(req).GetAwaiter().GetResult();
//            if (tenantId == null)
//            {
//                Fail(context, 400, "ERR_MISSING_TENANT_ID", "Missing tenantId in body");
//                return;
//            }

//            // 3) Decode JWT (no signature validate yet)
//            JwtSecurityToken jwt;
//            try
//            {
//                jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
//            }
//            catch
//            {
//                Fail(context, 401, "ERR_INVALID_JWT_FORMAT", "Invalid JWT format");
//                return;
//            }

//            // 4) Check "Tenants" claim
//            var tenantsClaim = jwt.Claims.FirstOrDefault(x => x.Type == "Tenants")?.Value;
//            if (tenantsClaim == null)
//            {
//                Fail(context, 401, "ERR_MISSING_TENANTS_CLAIM", "Missing Tenants claim in token");
//                return;
//            }

//            var allowedTenants = tenantsClaim.Split(',', StringSplitOptions.RemoveEmptyEntries);
//            if (!allowedTenants.Contains(tenantId, StringComparer.OrdinalIgnoreCase))
//            {
//                Fail(context, 401, "ERR_TENANT_NOT_ALLOWED", "Tenant not authorized for this token");
//                return;
//            }

//            // 5) Get AppId claim
//            var appIdClaim = jwt.Claims.FirstOrDefault(x => x.Type == "AppId")?.Value;
//            if (string.IsNullOrWhiteSpace(appIdClaim))
//            {
//                Fail(context, 401, "ERR_MISSING_APPID", "Missing appId claim");
//                return;
//            }

//            // 6) Load App info via gRPC
//            var appsSvc = context.HttpContext.RequestServices.GetRequiredService<IAppsGrpcClientAppService>();

//            var apps = appsSvc.AppsClient()
//                              .AppsByPublicIdRow(new AppsByPublicIdRowRpcRequest
//                              {
//                                  lang_id = 1,
//                                  public_id = Guid.Parse(appIdClaim)
//                              })
//                              .Result;

//            if (apps == null || string.IsNullOrEmpty(apps.secret_key))
//            {
//                Fail(context, 401, "ERR_INVALID_APPID", "Invalid AppId or missing secretKey");
//                return;
//            }

//            // 7) Validate token signature
//            try
//            {
//                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apps.secret_key));
//                var handler = new JwtSecurityTokenHandler();

//                var parameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = false,
//                    ValidateAudience = false,
//                    ValidateLifetime = true,
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = key,
//                    ClockSkew = TimeSpan.Zero
//                };

//                var principal = handler.ValidateToken(token, parameters, out _);

//                // userType validation (optional)
//                if (RequiredUserType != null)
//                {
//                    var userType = principal.FindFirst("userType")?.Value;
//                    if (!string.Equals(userType, RequiredUserType, StringComparison.OrdinalIgnoreCase))
//                    {
//                        Fail(context, 401, "ERR_INVALID_USER_TYPE", "User type not authorized");
//                        return;
//                    }
//                }

//                context.HttpContext.User = principal;
//            }
//            catch (SecurityTokenExpiredException)
//            {
//                Fail(context, 401, "ERR_TOKEN_EXPIRED", "Token expired");
//            }
//            catch (Exception)
//            {
//                Fail(context, 401, "ERR_TOKEN_INVALID", "Token invalid");
//            }
//        }

//        // ==========================
//        // Helper functions
//        // ==========================

//        private async Task<string?> ReadTenantId(HttpRequest request)
//        {
//            request.EnableBuffering();

//            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
//            string body = await reader.ReadToEndAsync();

//            request.Body.Position = 0;

//            if (string.IsNullOrWhiteSpace(body))
//                return null;

//            try
//            {
//                using var doc = JsonDocument.Parse(body);
//                var root = doc.RootElement;

//                foreach (var prop in root.EnumerateObject())
//                {
//                    if (string.Equals(prop.Name, "tenantId", StringComparison.OrdinalIgnoreCase) ||
//                        string.Equals(prop.Name, "tenant_id", StringComparison.OrdinalIgnoreCase))
//                    {
//                        return prop.Value.GetString();
//                    }
//                }

//            }
//            catch
//            {
//                return null;
//            }

//            return null;
//        }

//        private void Fail(AuthorizationFilterContext context, int status, string code, string msg)
//        {
//            context.Result = new ObjectResult(new
//            {
//                success = false,
//                errorCode = code,
//                errorMessage = msg
//            })
//            {
//                StatusCode = status
//            };
//        }
//    }
//}
