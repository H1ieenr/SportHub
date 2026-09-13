using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sporthub.app.contracts;
using sporthub.domain;
using Microsoft.AspNetCore.Identity;

namespace sporthub.app
{
    public static class CatalogAppServiceExtension
    {
        public static IServiceCollection AddCatalogMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(BrandMapperProfile));
            services.AddAutoMapper(typeof(CategoryMapperProfile));
            services.AddAutoMapper(typeof(ProductMapperProfile));
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

            return services;
        }
    }
}