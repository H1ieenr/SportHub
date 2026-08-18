using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Persistence;
using sporthub.domain;
namespace sporthub.repository
{
    public static class RepositoryServiceExtension
    {
        public static IServiceCollection AddSportHubRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Connection string 'SportHubDatabase' was not found.");

            services.AddDbContext<SportHubDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped(typeof(IGenericRepository<>), typeof(SportHubGenericRepository<>));
            services.AddScoped<IUnitOfWork<SportHubDbContext>, SportHubUnitOfWork>();
            services.AddScoped<ISportHubUnitOfWork, SportHubUnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();


            services.AddIdentityRepository();
            services.AddCatalogRepository();
            return services;
        }
    }
}