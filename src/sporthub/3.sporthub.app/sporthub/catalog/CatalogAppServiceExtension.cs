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
            return services;
        }

        public static IServiceCollection AddCatalogAppService(this IServiceCollection services)
        {
            services.AddCatalogMapper();

            #region brand
            services.AddScoped<IBrandAppService, BrandAppService>();
            #endregion

            return services;
        }
    }
}