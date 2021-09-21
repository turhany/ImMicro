using System;
using Hangfire;
using Hangfire.PostgreSql;
using ImMicro.Common.Constans; 
using ImMicro.ScheduleService.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImMicro.ScheduleService.Configurations
{
    /// <summary>
    /// Health check configuration extension
    /// </summary>
    public static class ConfigureHangfire
    {
        /// <summary>
        /// Add Health check configuration extension
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddHangfireConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var hangfireConnectionString = configuration.GetConnectionString(AppConstants.PostgreSqlConnectionString);
            services.AddHangfire(config =>
            {
                var option = new PostgreSqlStorageOptions
                {
                    PrepareSchemaIfNecessary = true,
                    QueuePollInterval = TimeSpan.FromMinutes(5),
                    InvisibilityTimeout = TimeSpan.FromHours(3)
                };

                config.UsePostgreSqlStorage(hangfireConnectionString, option).WithJobExpirationTimeout(TimeSpan.FromHours(6));
            });

            services.AddHangfireServer();
        
            return services;
        }

        /// <summary>
        /// Use Health check configuration
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseHangfireConfiguration(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                DashboardTitle = "ImMicro HangFire DashBoard",
                AppPath = "/hangfire",
                IgnoreAntiforgeryToken = true,
                Authorization = new []{ new DashboardNoAuthorizationFilter() }
            });

            return app;
        }
    }
}