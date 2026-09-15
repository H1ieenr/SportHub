using Microsoft.Extensions.DependencyInjection;
using sporthub.domain;

namespace sporthub.repository
{
    public static class CatalogRepositoryServiceExtension
    {
        public static IServiceCollection AddCatalogRepository(this IServiceCollection services)
        {
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            return services;
        }
    }
}