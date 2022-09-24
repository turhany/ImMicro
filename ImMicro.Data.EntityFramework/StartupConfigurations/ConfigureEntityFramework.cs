using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.DependencyInjection; 

namespace ImMicro.Data.EntityFramework.StartupConfigurations
{
    /// <summary>
    /// Entity Framework configuration extension
    /// </summary>
    public static class ConfigureEntityFramework
    {
        /// <summary>
        /// Add Entity Framework configuration
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <returns></returns>
        public static IServiceCollection AddEntityFrameworkConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseNpgsql(connectionString);
            });

            using (var serviceScope = services.BuildServiceProvider().GetService<IServiceScopeFactory>().CreateScope())
            {
                var dataBaseService = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                if (dataBaseService != null && dataBaseService.Database != null)
                {
                    dataBaseService.Database.Migrate();
                }
            }

            return services;
        } 
    }
}