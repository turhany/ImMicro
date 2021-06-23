using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ImMicro.Common.Constans;

namespace ImMicro.Api.Configurations.Startup
{
    /// <summary>
    /// Health check configuration extension
    /// </summary>
    public static class ConfigureHealthCheck
    {
        /// <summary>
        /// Add Health check configuration extension
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddNpgSql(configuration.GetConnectionString(AppConstants.DbConnectionString))
                .AddRedis(configuration.GetConnectionString(AppConstants.RedisConnectionString));

            services.AddHealthChecksUI()
                .AddInMemoryStorage();


            return services;
        }

        /// <summary>
        /// Use Health check configuration
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseHealthCheckConfiguration(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health-check", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(p => { p.UIPath = "/health-check-ui"; });

            return app;
        }
    }
}