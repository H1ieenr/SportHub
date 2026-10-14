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
            services.AddScoped<ICategoryOptionRepository, CategoryOptionRepository>();
            services.AddScoped<IOptionDefinitionRepository, OptionDefinitionRepository>();
            services.AddScoped<IOptionValueRepository, OptionValueRepository>();
            services.AddScoped<IProductVariantOptionValueRepository, ProductVariantOptionValueRepository>();
            return services;
        }
    }
}