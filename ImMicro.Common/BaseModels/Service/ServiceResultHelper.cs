using System.Collections.Generic;
using AutoMapper;
using Filtery.Models;
using ImMicro.Common.Extensions;
using ImMicro.Common.Pager;
using ImMicro.Common.Validation.Abstract;
using ImMicro.Resources.Service;

namespace ImMicro.Common.BaseModels.Service
{
    public class ServiceResultHelper
    {
        /// <summary>
        /// Returns with mapped data
        /// </summary>
        /// <typeparam name="TResponse">The response generic type</typeparam>
        /// <param name="data">The given data</param>
        /// <param name="mapper">Auto Mapper</param>
        /// <returns>The mapped data</returns>
        public static ServiceResult<TResponse> CreateSuccessResult<TResponse>(object data, IMapper mapper)
            where TResponse : class
            => new()
            {
                Status = ResultStatus.Successful,
                Data = mapper.Map<TResponse>(data)
            };

        /// <summary>
        /// Returns with mapped data
        /// </summary>
        /// <typeparam name="TMapping">The mapping class.</typeparam>
        /// <param name="data">The given data</param>
        /// <param name="mapper">Auto Mapper</param>
        /// <returns>The mapped data</returns>
        public static ServiceResult<dynamic> CreateSuccessDynamicResult<TMapping>(object data, IMapper mapper)
            where TMapping : class
            => new()
            {
                Status = ResultStatus.Successful,
                Data = mapper.Map<TMapping>(data)
            };

        /// <summary>
        /// Returns with mapped data
        /// </summary>
        /// <param name="data">The given data</param>
        /// <returns>The mapped data</returns>
        public static ServiceResult<dynamic> CreateSuccessResult(object data)
            => new()
            {
                Status = ResultStatus.Successful,
                Data = data
            };

        /// <summary>
        /// Returns data with success message
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="data">The given data</param>
        /// <param name="message">The given message</param>
        /// <returns>The dynamic data with success message</returns>
        public static ServiceResult<TResponse> CreateSuccessResult<TResponse>(dynamic data, string message)
            where TResponse : class
            => new()
            {
                Status = ResultStatus.Successful,
                Message = message,
                Data = data
            };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultStatus"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ServiceResult<T> CreateResult<T>(ResultStatus resultStatus, string message) where T : class
            => new()
            {
                Status = resultStatus,
                Message = message
            };

        /// <summary>
        /// If entity is null returns <c>true</c> with not found message, entity is not null returns <c>false</c>
        /// </summary>
        /// <typeparam name="TResponse">The response generic type</typeparam>
        /// <param name="entity">The entity</param>
        /// <param name="message">The message</param>
        /// <returns></returns>
        public static (ServiceResult<TResponse>, bool) CreateNotFoundResult<TResponse>(object entity, string message) where TResponse : class
        {
            if (entity == null)
            {
                var result = new ServiceResult<TResponse>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = message
                };
                return (result, true);
            }

            return (null, false);
        }

        /// <summary>
        /// CreateValidationResult
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="validationService"></param>
        /// <returns></returns>
        public static (ServiceResult<TResponse>, bool) CreateValidationResult<T, TResponse>(dynamic request,
            IValidationService validationService) where TResponse : class
        {
            var validationResponse = validationService.Validate(typeof(T), request);

            if (!validationResponse.IsValid)
            {
                var result = new ServiceResult<TResponse>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
                return (result, true);
            }

            return (null, false);
        }

        /// <summary>
        /// Returns with mapped data and pagination informations.
        /// </summary>
        /// <typeparam name="TResponse">The response generic type</typeparam>
        /// <typeparam name="TEntity">The entity generic type</typeparam>
        /// <param name="filteryResponse">The <see cref="FilteryResponse{T}"/> filtery response.</param>
        /// <returns>The mapped data and pagination informations. <see cref="PagedList{T}"/></returns>
        public static PagedList<TResponse> CreatePagedListResponse<TResponse, TEntity>(
            FilteryResponse<TEntity> filteryResponse, IMapper mapper)
            where TResponse : class
            where TEntity : class
            => new()
            {
                Data = mapper.Map<List<TResponse>>(filteryResponse.Data),
                PageInfo = filteryResponse.GetPageInfo()
            };
    }
}