using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ImMicro.Api.Configurations.Startup
{
    /// <summary>
    /// Configure CORS
    /// </summary>
    public static class ConfigureCORS
    {
        /// <summary>
        /// Add CORS Configurations
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsConfigurations(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", options =>
                {
                    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            return services;
        }

        /// <summary>
        /// Use CORS Configuration
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app)
        {
            app.UseCors("AllowOrigin");

            return app;
        }
    }
}