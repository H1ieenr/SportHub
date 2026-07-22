
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using demo_project.domain;

namespace demo_project.repository
{
    public static class RepositoryServiceExtension
    {
        public static IServiceCollection AddZaloAppRepository(this IServiceCollection services, IConfiguration configuration)
        {

            //services.AddScoped<IBlogRepository, BlogRepository>();
            //services.AddScoped<IHelpRepository, HelpRepository>();
            //services.AddScoped<IHelpFrontRepository, HelpFrontRepository>();
            //services.AddScoped<IRouteFrontRepository, RouteFrontRepository>();
            services.AddMiniCdpAppRepository();
            // 🔥 REGISTER DAPPER TYPE HANDLER
            SqlMapper.AddTypeHandler(new JsonElementTypeHandler());
            //_ = services.AddScoped<ISqlConnectionBuilder, DefaultSqlConnectionBuilder>(sp =>
            //    new DefaultSqlConnectionBuilder("Data Source= 103.77.167.46;Initial Catalog=dev_cloud_apps;MultipleActiveResultSets=True;TrustServerCertificate=True;User ID=acc_dev;Password=2wsxXSW@12345;"));
            //_ = services.AddScoped<SqlConnectionWrapperFactory>();
           
            return services;
        }
    }
}
