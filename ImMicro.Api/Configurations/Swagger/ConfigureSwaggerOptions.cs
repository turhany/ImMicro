using ImMicro.Common.Constans;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ImMicro.Api.Configurations.Swagger
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>, IConfigureOptions<SwaggerUIOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        /// <summary>
        /// Gets the open api information.
        /// </summary>
        /// <param name="description">The <see cref="ApiVersionDescription"/></param>
        /// <returns></returns>
        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "ImMicro API",
                Version = description.ApiVersion.ToString(),
                Description = "ImMicro Web API",
                Contact = new OpenApiContact { Name = "ImMicro Support Team", Email = SwaggerConstants.SupportEmailAddress },
                // License = new OpenApiLicense { Name = "ImMicro End User License", Url = new Uri(SwaggerConstants.HeadContent) },
                // TermsOfService = new Uri(SwaggerConstants.HeadContent) 
            };

            if (description.IsDeprecated)
                info.Title += " - This API version has been 'DEPRECATED'.";

            return info;
        }

        /// <summary>
        /// Configure the swagger ui configurations.
        /// </summary>
        /// <param name="options">The swagger ui options.</param>
        public void Configure(SwaggerUIOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var version in _provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"{SwaggerConstants.ApiRoutePrefixWithSlash}/{version.GroupName}/swagger.json",
                    $"{AppConstants.ProductName} Api {version.GroupName}");
            }

            options.DocumentTitle = AppConstants.ProductName;
            options.DocExpansion(DocExpansion.None);
            options.RoutePrefix = SwaggerConstants.ApiRoutePrefix;
        }
    }
}