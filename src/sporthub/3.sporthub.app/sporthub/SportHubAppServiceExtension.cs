using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sporthub.app.contracts;

namespace sporthub.app
{
    public static class SportHubAppServiceExtension
    {
        public static IServiceCollection AddSportHubAppServiceExtension(this IServiceCollection services, IConfiguration configuration)
        {
            //Helper Service
            services.AddScoped<IEnumHelperService, EnumHelperService>();

            //services.AddAutoMapper(typeof(BaseMapperProfile));
            services.AddIdentityAppService(configuration);
            services.AddCatalogAppService();
            return services;
        }
    }
}