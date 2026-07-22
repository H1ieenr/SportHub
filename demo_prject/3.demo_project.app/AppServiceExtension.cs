
using demo_project.app;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using demo_project.app;
using demo_project.app.contracts;

namespace demo_project.app
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddMiniFormAppService(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddWebAppService(configuration);
            services.AddWebAppService(configuration);
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(BaseMapperProfile));
            
            //services.AddGlobalCache(configuration.GetConnectionString("redis"));

            return services;
        }
    }
}
