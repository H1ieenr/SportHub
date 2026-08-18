using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sporthub.app.contracts;
using sporthub.domain;
using Microsoft.AspNetCore.Identity;
namespace sporthub.app
{
    public static class IdentityAppServiceExtension
    {
        public static IServiceCollection AddIdentityMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UsersMapperProfile));
            return services;
        }

        public static IServiceCollection AddIdentityAppService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityMapper();

            #region auth
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddScoped<ICustomerAuthAppService, CustomerAuthAppService>();
            services.AddScoped<IAdminAuthAppService, AdminAuthAppService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthCommonAppService, AuthCommonAppService>();
            services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
            #endregion

            #region users
            services.AddScoped<IUsersAppService, UsersAppService>();
            #endregion
            return services;
        }
    }
}