using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ImMicro.Api.Configurations.Startup
{
    /// <summary>
    /// Configure Api Versioning
    /// </summary>
    public static class ConfigureApiVersioning
    {
        /// <summary>
        /// Add Api Versioning Configurations
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApiVersioningConfigurations(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
            });
            
            return services;
        }

        /// <summary>
        /// Use Api Versioning Configuration
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiVersioningConfiguration(this IApplicationBuilder app)
        {
            app.UseApiVersioning();

            return app;
        }
    }
}