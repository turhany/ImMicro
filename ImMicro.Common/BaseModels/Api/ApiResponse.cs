using System.Collections.Generic;
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
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.SUCCESS,
                        Message = result.Message,
                        Data = result.Data,
                        ValidationMessages = result.ValidationMessages
                    })
                    { StatusCode = 200 };
                case ResultStatus.Created:
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.SUCCESS,
                        Message = result.Message,
                        Data = result.Data,
                        ValidationMessages = result.ValidationMessages
                    })
                    { StatusCode = 201 };
                case ResultStatus.Accepted:
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.SUCCESS,
                        Message = result.Message,
                        ValidationMessages = result.ValidationMessages
                    })
                    { StatusCode = 202 };
                case ResultStatus.InvalidInput:
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.INPUT_ERROR,
                        Message = result.Message,
                        ValidationMessages = result.ValidationMessages
                    })
                    { StatusCode = 400 };
                case ResultStatus.Exists:
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.INPUT_ERROR,
                        Message = result.Message,
                        ValidationMessages = new List<string>() { result.Message }
                    })
                    { StatusCode = 400 };
                case ResultStatus.BadRequest:
                    return new ObjectResult( new DataResponse
                    {
                        Status = ServiceResponseStatus.FAILED,
                        Message = result.Message,
                        ValidationMessages = result.ValidationMessages
                    } )
                    { StatusCode = 400 };
                case ResultStatus.Forbidden:
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.FAILED,
                        Message = result.Message,
                        ValidationMessages = result.ValidationMessages
                    })
                    { StatusCode = 403 };
                case ResultStatus.ResourceNotFound:
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.FAILED,
                        Message = result.Message,
                        ValidationMessages = result.ValidationMessages
                    })
                    { StatusCode = 404 };
                case ResultStatus.ErrorOccurred:
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.FAILED,
                        Message = result.Message,
                        ValidationMessages = result.ValidationMessages
                    })
                    { StatusCode = 500 };
                case ResultStatus.Failed:
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.FAILED,
                        Message = result.Message,
                        ValidationMessages = result.ValidationMessages
                    })
                    { StatusCode = 500 };
                default:
                    return new ObjectResult(new DataResponse
                    {
                        Status = ServiceResponseStatus.FAILED,
                        Message = ServiceResponseMessage.FAILED,
                        ValidationMessages = result.ValidationMessages
                    })
                    { StatusCode = 500 };
            }
        }
        
        public static readonly ObjectResult InvalidInputResult = new ObjectResult(new BaseResponse()
                {
                    Message = ServiceResponseMessage.INVALID_REQUEST,
                    Status = ServiceResponseStatus.INPUT_ERROR
                })
                { StatusCode = 400 };
    }
}