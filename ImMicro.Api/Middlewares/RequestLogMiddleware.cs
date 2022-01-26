using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImMicro.Business.RequestLog.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection; 
using ImMicro.Model.RequestLog;

namespace ImMicro.Api.Middlewares
{
    /// <summary>
    /// Global request - response log middleware
    /// </summary>
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory; 

        private readonly List<string> _ignoreFileForLog = new List<string>
        {
            ".png",
            ".html",
            "swagger.json",
            "health-check"
        };

        /// <summary>
        /// Global request - response log middleware constructor
        /// </summary>
        /// <param name="next">Next middleware</param>
        /// <param name="serviceScopeFactory">Service Scope Factory</param>
        public RequestLogMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory; 
        }

        /// <summary>
        /// Middleware invoke method
        /// </summary>
        /// <param name="httpContext">Http Context</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await WriteLogAsync(httpContext);
            }
            catch
            {
                //ignored
            }
        }

        private async Task WriteLogAsync(HttpContext httpContext)
        { 
            var request = await ReadRequestBodyAsync(httpContext);
            var originalBodyStream = httpContext.Response.Body;
            await using var responseBody = new MemoryStream();
            httpContext.Response.Body = responseBody;

            await _next(httpContext);
            
            var response = await ReadResponseBodyAsync(httpContext.Response);

            var requestLog = new RequestLog
            {
                Request = request,
                Response = response,
                StatusCode = httpContext.Response.StatusCode.ToString(),
                RequestPath = httpContext.Request.Path.Value
            };
 
            _ = Task.Run(async () => await SaveRequestLogAsync(requestLog, _serviceScopeFactory));

            await responseBody.CopyToAsync(originalBodyStream);
        }

        private static async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            if (context.Request.ContentLength == null || context.Request.ContentLength == 0)
                return string.Empty;

            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, Convert.ToInt32(context.Request.ContentLength), true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            return body;
        }

        private static async Task<string> ReadResponseBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body, Encoding.UTF8).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            
            return $"{response.StatusCode}: {text}";
        }

        private async Task SaveRequestLogAsync(RequestLog requestLog, IServiceScopeFactory serviceScopeFactory)
        {
            if ( !string.IsNullOrWhiteSpace(requestLog.RequestPath) && _ignoreFileForLog.Any(p => requestLog.RequestPath.Contains(p, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }
            
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var requestLogService = (IRequestLogService) scope.ServiceProvider.GetRequiredService(typeof(IRequestLogService));
                await requestLogService.SaveAsync(requestLog);
            }
        }
    }
}