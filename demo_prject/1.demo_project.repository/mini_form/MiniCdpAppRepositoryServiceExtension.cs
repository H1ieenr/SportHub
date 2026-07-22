using demo_project.domain;
using Microsoft.Extensions.DependencyInjection;

namespace demo_project.repository
{
    public static class MiniCdpAppRepositoryServiceExtension
    {
        public static IServiceCollection AddMiniCdpAppRepository(this IServiceCollection services)
        {
            services.AddMiniCdpInternalRepository();
            services.AddMiniCdpWebRepository();
            return services;
        }
    }
}