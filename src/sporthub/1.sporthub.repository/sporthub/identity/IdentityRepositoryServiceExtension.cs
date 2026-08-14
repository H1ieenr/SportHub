using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sporthub.domain;
namespace sporthub.repository
{
    public static class IdentityRepositoryServiceExtension
    {
        public static IServiceCollection AddIdentityRepository(this IServiceCollection services)
        {
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            return services;
        }
    }
}