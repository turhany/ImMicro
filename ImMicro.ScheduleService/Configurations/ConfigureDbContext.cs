using ImMicro.Common.Constans;
using ImMicro.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImMicro.ScheduleService.Configurations
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
                options.UseNpgsql(configuration.GetConnectionString(AppConstants.PostgreSqlConnectionString))
                    .EnableSensitiveDataLogging();
                options.UseLazyLoadingProxies();
            });

            return services;
        }
    }
}