using System;
using System.Net;
using System.Threading.Tasks;
using Filtery.Exceptions;
using ImMicro.Common.BaseModels.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ImMicro.Common.Constans;  
using ImMicro.Resources.App;
using ImMicro.Lock.Exceptions;

namespace ImMicro.Api.Middlewares
{
  /// <summary>
    /// Global Exception handler middleware
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Global Exception handler middleware constructor
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="next">Next middleware</param>
        public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        /// <summary>
        /// Middleware invoke method
        /// </summary>
        /// <param name="httpContext">Http Context</param>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var response = new BaseResponse();
            try
            {
                await _next(httpContext);
            }
            catch (FilteryBaseException ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = ServiceResponseStatus.FAILED;
                response.Message = ex.Message;

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                httpContext.Response.ContentType = AppConstants.JsonContentType;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (AcquireLockException ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = ServiceResponseStatus.FAILED;
                response.Message = ex.Message;

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                httpContext.Response.ContentType = AppConstants.JsonContentType;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.Status = ServiceResponseStatus.INTERNAL_SERVER_ERROR;
                response.Message = ServiceResponseStatus.INTERNAL_SERVER_ERROR;

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = AppConstants.JsonContentType;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}