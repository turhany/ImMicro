using ImMicro.Common.Constans;
using ImMicro.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImMicro.Common.StartupConfigurations
{
    /// <summary>
    /// Add Swagger configuration extension
    /// </summary>
    public static class ConfigureOptions
    {
        /// <summary>
        /// Add Swagger configuration extension
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddOptionConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<RedLockOption>(configuration.GetSection(AppConstants.RedLockSettingsOptionName));
            return services;
        }
    }
}