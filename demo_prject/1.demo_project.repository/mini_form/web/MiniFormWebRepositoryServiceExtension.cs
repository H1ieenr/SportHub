using demo_project.domain;
using Microsoft.Extensions.DependencyInjection;

namespace demo_project.repository
{
    public static class MiniCdpWebRepositoryServiceExtension
    {
        public static IServiceCollection AddMiniCdpWebRepository(this IServiceCollection services)
        {
            services.AddScoped<IBaseFieldWebRepository, BaseFieldWebRepository>();
            //services.AddScoped<IProviderAppRepository, ProviderAppRepository>();
            //services.AddScoped<ISagaStateRepository, SagaStateRepository>();
            //services.AddScoped<IConsumerInternalRepository, ConsumerInternalRepository>();
            //services.AddScoped<IConsumerVoucherInternalRepository, ConsumerVoucherInternalRepository>();
            //services.AddScoped<IConditionRuleRepository, ConditionRuleRepository>();
            return services;
        }
    }
}