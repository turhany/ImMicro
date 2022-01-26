using System;
using System.IO;
using System.Reflection;
using ImMicro.Api.Configurations.Swagger;
using ImMicro.Common.Constans;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

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
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.OperationFilter<SwaggerDefaultValues>();
                c.OperationFilter<SwaggerHttpHeaderFilter>();

                c.OrderActionsBy((apiDesc) =>
                    $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

                c.SchemaFilter<EnumSchemaFilter>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme (Example: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpX')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                c.EnableAnnotations();
            });

            services.ConfigureSwaggerGen(options => { options.CustomSchemaIds(x => x.FullName); });

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(SwaggerConstants.DefaultApiVersion, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

            return services;
        }

        /// <summary>
        /// Use Swagger configuration
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger(c => { c.RouteTemplate = "api/{documentName}/swagger.json"; });

            app.UseSwaggerUI();

            return app;
        }
    }
}