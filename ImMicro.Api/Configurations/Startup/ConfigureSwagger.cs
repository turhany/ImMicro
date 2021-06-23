using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ImMicro.Common.Constans;

namespace ImMicro.Api.Configurations.Startup
{
    /// <summary>
    /// Add Swagger configuration extension
    /// </summary>
    public static class ConfigureSwagger
    {
        /// <summary>
        /// Add Swagger configuration extension
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = AppConstants.SolutionName, Version = "v1" });
            });

            return services;
        }

        /// <summary>
        /// Use Swagger configuration
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppConstants.SolutionName} v1"); });

            return app;
        }
    }
}