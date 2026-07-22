using demo_project.domain;
using Microsoft.Extensions.DependencyInjection;

namespace demo_project.repository
{
    public static class MiniCdpInternalRepositoryServiceExtension
    {
        public static IServiceCollection AddMiniCdpInternalRepository(this IServiceCollection services)
        {
            //services.AddScoped<IChannelAppRepository, ChannelAppRepository>();
            //services.AddScoped<IProviderAppRepository, ProviderAppRepository>();
            //services.AddScoped<ISagaStateRepository, SagaStateRepository>();
            //services.AddScoped<IConsumerInternalRepository, ConsumerInternalRepository>();
            //services.AddScoped<IConsumerVoucherInternalRepository, ConsumerVoucherInternalRepository>();
            //services.AddScoped<IConditionRuleRepository, ConditionRuleRepository>();
            return services;
        }
    }
}