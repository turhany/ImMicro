using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ImMicro.Common.Constans;
using ImMicro.Common.Dapper;
using ImMicro.Data;
// ReSharper disable PossibleNullReferenceException

namespace ImMicro.Api.Configurations.Startup
{
    /// <summary>
    /// Database configuration extension
    /// </summary>
    public static class ConfigureDbContext
    {
        /// <summary>
        /// Add DataBase configurations
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseNpgsql(configuration.GetConnectionString(AppConstants.PostgreSqlConnectionString));
            });

            using (var serviceScope = services.BuildServiceProvider().GetService<IServiceScopeFactory>().CreateScope())
            {
                var dataBaseService = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                if (dataBaseService != null && dataBaseService.Database != null)
                {
                    dataBaseService.Database.Migrate();
                }
            }
            
            services.AddSingleton(
                new DapperContext(configuration.GetConnectionString(AppConstants.PostgreSqlConnectionString), 
                    DapperContext.ConnectionType.PostgreSql));

            return services;
        }
    }
}