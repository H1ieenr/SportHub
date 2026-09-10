using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sporthub.app.contracts;   
namespace sporthub.app
{
    public static class ThirdPartyAppServiceExtension
    {
        public static IServiceCollection AddThirdPartyAppService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            return services;
        }
    }
}