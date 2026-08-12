using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sporthub.app
{
    public static class AppServiceExtension
    {
        public static IServiceCollection AddZaloMapper(this IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(WalletInstanceZaloMapperProfile));
            return services;
        }
        public static IServiceCollection AddAppService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddZaloMapper();

            //services.AddAutoMapper(typeof(BaseMapperProfile));
            

            return services;
        }
    }
}