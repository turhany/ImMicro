using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ImMicro.Api.Configurations.Swagger
{
    /// <summary>
    /// <see cref="SwaggerHttpHeaderFilter"/>
    /// </summary>
    public class SwaggerHttpHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// Apply http header operation.
        /// </summary>
        /// <param name="operation">The <see cref="OpenApiOperation"/> instance.</param>
        /// <param name="context">The <see cref="OperationFilterContext"/> instance.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Description = "Please, set language.",
                Required = false
            });
        }
    }
}