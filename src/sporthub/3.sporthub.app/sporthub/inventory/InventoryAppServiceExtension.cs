using Microsoft.Extensions.DependencyInjection;
using sporthub.app.contracts;

namespace sporthub.app.sporthub
{
    public static class InventoryAppServiceExtension
    {
        public static IServiceCollection AddInventoryMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(InventoryMapperProfile));
            return services;
        }

        public static IServiceCollection AddInventoryAppService(this IServiceCollection services)
        {
            #region Inventory
            services.AddScoped<IInventoryAppService, InventoryAppService>();
            #endregion

            return services;
        }
    }
}
