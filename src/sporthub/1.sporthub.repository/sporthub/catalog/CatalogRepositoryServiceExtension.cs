using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sporthub.domain;

namespace sporthub.repository
{
    public static class CatalogRepositoryServiceExtension
    {
        public static IServiceCollection AddCatalogRepository(this IServiceCollection services)
        {
            services.AddScoped<IBrandRepository, BrandRepository>();
            return services;
        }
    }
}