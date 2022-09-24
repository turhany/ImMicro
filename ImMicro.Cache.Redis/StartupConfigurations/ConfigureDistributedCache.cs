using Microsoft.Extensions.DependencyInjection; 

namespace ImMicro.Cache.Redis.StartupConfigurations
{
    /// <summary>
    /// Distributed cache configuration extension
    /// </summary>
    public static class ConfigureDistributedCache
    {
        /// <summary>
        /// Add Distributed cache configuration
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns></returns>
        public static IServiceCollection AddDistributedCacheConfiguration(this IServiceCollection services, string connectionString, string instanceName)
        {
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = connectionString;
                options.InstanceName = instanceName;
            });
            return services;
        } 
    }
}