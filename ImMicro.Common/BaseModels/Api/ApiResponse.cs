using System.Collections.Generic;
using System.Net;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Resources.App;
using ImMicro.Resources.Service;
using Microsoft.AspNetCore.Mvc;

namespace ImMicro.Common.BaseModels.Api
{
    public static class ApiResponse
    {
         public static BaseResponse BuildBaseResponse(string status, string message = "")
        {
            return new BaseResponse
            {
                Status = status,
                Message = message
            };
        }
         
         public static ObjectResult CreateResult<T>(ServiceResult<T> result) where T : class
         {
             switch (result.Status)
             {
                 case ResultStatus.Successful:
                     return CreateObjectResult(HttpStatusCode.OK, ServiceResponseStatus.SUCCESS, result.Message, result.ValidationMessages, result.Data);
                 case ResultStatus.Created:
                     return CreateObjectResult(HttpStatusCode.Created, ServiceResponseStatus.SUCCESS, result.Message, result.ValidationMessages, result.Data);
                 case ResultStatus.Accepted:
                     return CreateObjectResult(HttpStatusCode.Accepted, ServiceResponseStatus.SUCCESS, result.Message, result.ValidationMessages);
                 case ResultStatus.InvalidInput:
                     return CreateObjectResult(HttpStatusCode.BadRequest, ServiceResponseStatus.INPUT_ERROR, result.Message, result.ValidationMessages);
                 case ResultStatus.Exists:
                     return CreateObjectResult(HttpStatusCode.BadRequest, ServiceResponseStatus.INPUT_ERROR, result.Message, new List<string> { result.Message });
                 case ResultStatus.BadRequest:
                     return CreateObjectResult(HttpStatusCode.BadRequest, ServiceResponseStatus.FAILED, result.Message, result.ValidationMessages);
                 case ResultStatus.Forbidden:
                     return CreateObjectResult(HttpStatusCode.Forbidden, ServiceResponseStatus.FAILED, result.Message, result.ValidationMessages);
                 case ResultStatus.ResourceNotFound:
                     return CreateObjectResult(HttpStatusCode.NotFound, ServiceResponseStatus.FAILED, result.Message, result.ValidationMessages);
                 case ResultStatus.ErrorOccurred:
                 case ResultStatus.Failed:
                     return CreateObjectResult(HttpStatusCode.InternalServerError, ServiceResponseStatus.FAILED, result.Message, result.ValidationMessages);
                 default:
                     return CreateObjectResult(HttpStatusCode.InternalServerError, ServiceResponseStatus.FAILED, ServiceResponseMessage.FAILED, result.ValidationMessages);
             }
         }

         public static readonly ObjectResult InvalidInputResult = new ObjectResult(new BaseResponse()
                {
                    Message = ServiceResponseMessage.INVALID_REQUEST,
                    Status = ServiceResponseStatus.INPUT_ERROR
                })
                { StatusCode = 400 };
                
        private static ObjectResult CreateObjectResult(HttpStatusCode httpStatusCode, string status, string message, List<string> validationMessages, object data = null)
        {
            return new ObjectResult(new DataResponse
            {
                Status = status,
                Message = message,
                Data = data,
                ValidationMessages = validationMessages
            })
            {
                StatusCode = (int) httpStatusCode
            };
        }

    }
}