using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sporthub.app
{
    public static class SportHubAppServiceExtension
    {
         public static IServiceCollection AddSportHubAppServiceExtension(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddAutoMapper(typeof(BaseMapperProfile));
            services.AddIdentityAppService(configuration);
            services.AddCatalogAppService();
            return services;
        }
    }
}