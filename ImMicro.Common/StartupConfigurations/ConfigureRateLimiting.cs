using AspNetCoreRateLimit;
using ImMicro.Common.Constans;
using ImMicro.Common.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImMicro.Common.StartupConfigurations
{
    /// <summary>
    /// Rate Limiting configuration extension
    /// </summary>
    public static class ConfigureRateLimiting
    {
        /// <summary>
        /// Add Rate Limitig configuration extension
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var rateLimitingMode = configuration.GetValue<RateLimitingMode>(AppConstants.RateLimitingModeOptionName);

            switch (rateLimitingMode)
            {
                case RateLimitingMode.InMemoryMode:
                    services.AddMemoryCache();
                    services.Configure<IpRateLimitOptions>(configuration.GetSection(AppConstants.IPRateLimitingOptionName));
                    services.AddInMemoryRateLimiting();
                    services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
                    break;
                case RateLimitingMode.None:
                    break;
            }

            return services;
        }

        /// <summary>
        /// Use Rate Limiiting configuration
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseRateLimitingConfiguration(this IApplicationBuilder app, IConfiguration configuration)
        {
            var rateLimitingMode = configuration.GetValue<RateLimitingMode>(AppConstants.RateLimitingModeOptionName);
            if (rateLimitingMode != RateLimitingMode.None)
            {
                app.UseIpRateLimiting();
            }

            return app;
        }
    }
}