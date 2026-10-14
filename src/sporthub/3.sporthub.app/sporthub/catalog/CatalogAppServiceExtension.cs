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
            services.AddAutoMapper(typeof(CategoryOptionMapperProfile));
            services.AddAutoMapper(typeof(OptionDefinitionMapperProfile));
            services.AddAutoMapper(typeof(OptionValueMapperProfile));
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
            #region category option 
            services.AddScoped<ICategoryOptionAppService, CategoryOptionAppService>();
            #endregion
            #region option definition 
            services.AddScoped<IOptionDefinitionAppService, OptionDefinitionAppService>();
            #endregion
            #region option value 
            services.AddScoped<IOptionValueAppService, OptionValueAppService>();
            #endregion
            #region product
            services.AddScoped<IProductAppService, ProductAppService>();
            #endregion
            #region product variant
            services.AddScoped<IProductVariantAppService, ProductVariantAppService>();
            #endregion
            #region product variant option value
            services.AddScoped<IProductVariantOptionValueAppService, ProductVariantOptionValueAppService>();
            #endregion
            #region product image
            services.AddScoped<IProductImageAppService, ProductImageAppService>();
            #endregion

            return services;
        }
    }
}