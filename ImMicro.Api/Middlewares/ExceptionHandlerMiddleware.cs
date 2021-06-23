using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ImMicro.Common.Constans; 
using ImMicro.Common.Resources;
using ImMicro.Contract.Dtos;

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
            var response = new BaseModel();
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, ex.Message);
                response.HasError = true;
                response.ErrorCode = ((int)HttpStatusCode.BadRequest).ToString();
                response.ErrorMessages = ex.Errors.Select(p => p.ErrorMessage).ToList();
                response.Message = Literals.ValidationError_Message;
                
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                httpContext.Response.ContentType = AppConstants.JsonContentType;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                response.HasError = true;
                response.ErrorCode = ((int)HttpStatusCode.InternalServerError).ToString();
                response.ErrorMessages.Add(Literals.ServerError_Message);
                response.Message = Literals.ServerError_Message;

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = AppConstants.JsonContentType;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}