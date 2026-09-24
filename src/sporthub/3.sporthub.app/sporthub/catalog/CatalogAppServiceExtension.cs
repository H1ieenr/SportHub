using Microsoft.Extensions.DependencyInjection;
using sporthub.app.contracts;

namespace sporthub.app
{
    public static class CatalogAppServiceExtension
    {
        public static IServiceCollection AddCatalogMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(BrandMapperProfile));
            services.AddAutoMapper(typeof(CategoryMapperProfile));
            services.AddAutoMapper(typeof(ProductMapperProfile));
            services.AddAutoMapper(typeof(ProductVariantMapperProfile));
            services.AddAutoMapper(typeof(ProductImageMapperProfile));
            return services;
        }

        public static IServiceCollection AddCatalogAppService(this IServiceCollection services)
        {
            services.AddCatalogMapper();

            #region brand
            services.AddScoped<IBrandAppService, BrandAppService>();
            #endregion
            #region category
            services.AddScoped<ICategoryAppService, CategoryAppService>();
            #endregion
            #region product
            services.AddScoped<IProductAppService, ProductAppService>();
            #endregion
            #region product variant
            services.AddScoped<IProductVariantAppService, ProductVariantAppService>();
            #endregion
            #region product image
            services.AddScoped<IProductImageAppService, ProductImageAppService>();
            #endregion

            return services;
        }
    }
}