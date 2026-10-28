using Microsoft.Extensions.DependencyInjection;
using sporthub.domain;

namespace sporthub.repository
{
    public static class InventoryRepositoryServiceExtension
    {
        public static IServiceCollection AddInventoryRepository(this IServiceCollection services)
        {
            services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
            services.AddScoped<IInventoryTransactionRepository, InventoryTransactionRepository>();
            return services;
        }
    }
}
