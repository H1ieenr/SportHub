using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common;

namespace sporthub.api
{
    public static class AuthorizeAppServiceExtension
    {
        public static IServiceCollection AddAuthorizeAppService(this IServiceCollection services, IConfiguration configuration)
        {

            var jwtSettings = configuration.GetSection("JwtSettings")
            ?? throw new InvalidOperationException("JwtSettings chưa được cấu hình.");
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AppPolicies.AdminAndStaff, policy =>
                    policy.RequireAssertion(ctx =>
                        ctx.User.HasClaim(c => c.Type == "aud" && c.Value == jwtSettings["AdminAudience"])));

                options.AddPolicy(AppPolicies.Admin, policy =>
                    policy.RequireAssertion(ctx =>
                        ctx.User.HasClaim(c => c.Type == "aud" && c.Value == jwtSettings["AdminAudience"]) &&
                        ctx.User.IsInRole(AppPolicies.Admin)));

                options.AddPolicy(AppPolicies.Staff, policy =>
                    policy.RequireAssertion(ctx =>
                        ctx.User.HasClaim(c => c.Type == "aud" && c.Value == jwtSettings["AdminAudience"]) &&
                        ctx.User.IsInRole(AppPolicies.Staff)));

                options.AddPolicy(AppPolicies.Customer, policy =>
                   policy.RequireAssertion(ctx =>
                       ctx.User.HasClaim(c => c.Type == "aud" && c.Value == jwtSettings["CustomerAudience"])));
            });
            return services;
        }
    }
}