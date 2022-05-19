using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Compression;

namespace ImMicro.Common.StartupConfigurations
{
    /// <summary>
    /// Distributed cache configuration extension
    /// </summary>
    public static class ConfigureCompression
    {
        /// <summary>
        /// Add Compression configuration
        /// </summary>
        /// <param name="services">ServiceCollection</param>        
        /// <returns></returns>
        public static IServiceCollection AddCompressionConfiguration(this IServiceCollection services)
        {
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>
            (opt =>
            {
                opt.Level = CompressionLevel.Fastest;
            });
            return services;
        }

        /// <summary>
        /// Use Compression configuration
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseCompressionConfiguration(this IApplicationBuilder app)
        {
            app.UseResponseCompression();
            return app;
        }
    }
}