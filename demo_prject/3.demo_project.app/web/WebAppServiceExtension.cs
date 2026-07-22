
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using demo_project.app.contracts;
using demo_project.domain;

namespace demo_project.app
{
    public static class WebAppServiceExtension
    {
        public static IServiceCollection AddWebAppMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(BaseFieldWebMapperProfile));
            return services;
        }
        public static IServiceCollection AddWebAppService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBaseFieldWebAppService, BaseFieldWebAppService>();
            services.AddWebAppMapper(configuration);
            return services;
        }
    }
}
