using Microsoft.Extensions.DependencyInjection; 

namespace ImMicro.Cache.InMemory.StartupConfigurations
{
    /// <summary>
    /// InMemory cache configuration extension
    /// </summary>
    public static class ConfigureInMemoryCache
    {
        /// <summary>
        /// Add InMemory cache configuration
        /// </summary>
        /// <param name="services">ServiceCollection</param> 
        /// <returns></returns>
        public static IServiceCollection AddInMemoryCacheConfiguration(this IServiceCollection services)
        {
            services.AddMemoryCache();

            return services;
        } 
    }
}