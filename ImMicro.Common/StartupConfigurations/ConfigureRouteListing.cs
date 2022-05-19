using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Text.Json;

namespace ImMicro.Common.StartupConfigurations
{
    /// <summary>
    /// Route List configuration extension
    /// </summary>
    public static class ConfigureRouteListing
    {
        /// <summary>
        /// Use Route List configuration (use after > UseRouting)
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseRouteListConfiguration(this IApplicationBuilder app, string routeListEndpoint)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet(routeListEndpoint, request =>
                {
                    request.Response.Headers.Add("content-type", "application/json");

                    var ep = endpoints.DataSources.First().Endpoints.Select(e => e as RouteEndpoint);
                    return request.Response.WriteAsync(
                        JsonSerializer.Serialize(
                            ep.Select(e => new
                            {
                                Method = ((HttpMethodMetadata)e.Metadata.First(m => m.GetType() == typeof(HttpMethodMetadata))).HttpMethods.First(),
                                Route = e.RoutePattern.RawText
                            })
                        )
                    );
                });
            });

            return app;
        }
    }
}