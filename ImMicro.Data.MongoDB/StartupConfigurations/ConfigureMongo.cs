using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ImMicro.Data.MongoDB.Options;

namespace ImMicro.Data.MongoDB.StartupConfigurations
{
    /// <summary>
    /// Mongo configuration extension
    /// </summary>
    public static class ConfigureMongo
    {
        /// <summary>
        /// Add Mongo configuration
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <returns></returns>
        public static IServiceCollection AddMongoDBConfiguration(this IServiceCollection services, IConfiguration configuration, string configSectionName)
        {
            services.Configure<MongoDBOption>(configuration.GetSection(configSectionName));
            
            return services;
        } 
    }
}