using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sporthub.app
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddAppService(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddAutoMapper(typeof(BaseMapperProfile));
            services.AddSportHubAppServiceExtension(configuration);
            services.AddThirdPartyAppService(configuration);
            return services;
        }
    }
}