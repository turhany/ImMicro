using ImMicro.Lock.Redis.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImMicro.Lock.Redis.StartupConfigurations
{
    /// <summary>
    /// Distributed lock configuration extension
    /// </summary>
    public static class ConfigureDistributedLock
    {
        /// <summary>
        /// Add Distributed lock configuration
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedLockConfiguration(this IServiceCollection services, IConfiguration configuration, string configSectionName)
        {
            services.Configure<RedLockOption>(configuration.GetSection(configSectionName));
            
            return services;
        } 
    }
}