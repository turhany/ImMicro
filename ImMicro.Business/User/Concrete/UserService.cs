using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Filtery.Extensions;
using Filtery.Models;
using ImMicro.Business.User.Abstract;
using ImMicro.Business.User.Validator;
using ImMicro.Cache.Abstract;
using ImMicro.Cache.Constants;
using ImMicro.Common.Application;
using ImMicro.Common.Aspects;
using ImMicro.Common.Auth;
using ImMicro.Common.Auth.Concrete;
using ImMicro.Common.BaseModels.Service; 
using ImMicro.Common.Constans; 
using ImMicro.Common.Pager;
using ImMicro.Contract.App.User;
using ImMicro.Contract.Mappings.Filtery;
using ImMicro.Contract.Service.User;
using ImMicro.Data.BaseRepositories;
using ImMicro.Lock.Abstract;
using ImMicro.Resources.Extensions;
using ImMicro.Resources.Model;
using ImMicro.Resources.Service;
using ImMicro.Validation.Abstract;

namespace ImMicro.Business.User.Concrete
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<Model.User.User> _userRepository; 
        private readonly ICacheService _cacheService;
        private readonly ILockService _lockService;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public UserService(
            IGenericRepository<Model.User.User> userRepository,
            ICacheService cacheService, 
            ILockService lockService, 
            IMapper mapper, 
            IValidationService validationService)
        {
            _userRepository = userRepository; 
            _cacheService = cacheService;
            _lockService = lockService;
            _mapper = mapper;
            _validationService = validationService;
        }

        #region CRUD Operations

        public async Task<ServiceResult<UserView>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CacheKeyConstants.UserCacheKey, id);

            var user = await _cacheService.GetOrSetObjectAsync(cacheKey, 
                async () => await _userRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false, cancellationToken),
                CacheConstants.DefaultCacheDuration,
                cancellationToken);

            if (user == null)
            {
                return new ServiceResult<UserView>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(Entities.User)
                };
            }
            
            return new ServiceResult<UserView>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Retrieved(),
                Data = _mapper.Map<UserView>(user)
            };
        }
 
        public async Task<ServiceResult<ExpandoObject>> CreateAsync(CreateUserRequestServiceRequest request, CancellationToken cancellationToken)
        {
            var validationResponse = _validationService.Validate(typeof(CreateUserRequestValidator), request);

            if (!validationResponse.IsValid)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
            }

            if (await _userRepository.AnyAsync(p => p.Email.ToLower().Equals(request.Email.ToLower()) && p.IsDeleted == false, cancellationToken))
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = Resource.Duplicate(request.Email)
                };
            }

            var entity = new Model.User.User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Type = Model.User.UserType.Root //TODO: this need to be change as your logic
            };

            entity = await _userRepository.InsertAsync(entity, cancellationToken);

            dynamic userWrapper = new ExpandoObject();
            userWrapper.Id = entity.Id;

            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Created(Entities.User, entity.Email),
                Data = userWrapper
            };
        }

        public async Task<ServiceResult<ExpandoObject>> UpdateAsync(UpdateUserRequestServiceRequest request, CancellationToken cancellationToken)
        {
            var validationResponse = _validationService.Validate(typeof(UpdateUserRequestValidator), request);

            if (!validationResponse.IsValid)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
            }

            var entity = await _userRepository.FindOneAsync(p => p.Id == request.Id && p.IsDeleted == false, cancellationToken);

            if (entity == null)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(Entities.User)
                };
            }

            if (await _userRepository.AnyAsync(p => p.Id != request.Id && p.Email.ToLower().Equals(request.Email.ToLower()) && p.IsDeleted == false, cancellationToken))
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = Resource.Duplicate(entity.Email)
                };
            }

            var lockKey = string.Format(LockKeyConstants.UserLockKey, entity.Id);
            var cacheKey = string.Format(CacheKeyConstants.UserCacheKey, entity.Id);
            
            using (await _lockService.CreateLockAsync(lockKey, cancellationToken))
            {
                entity.FirstName = request.FirstName.Trim();
                entity.LastName = request.LastName.Trim();
                entity.Email = request.Email.Trim().ToLower();
                entity.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

                entity = await _userRepository.UpdateAsync(entity, cancellationToken);

                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
            }
            

            dynamic userWrapper = new ExpandoObject();
            userWrapper.Id = entity.Id;

            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Updated(Entities.User, entity.Email),
                Data = userWrapper
            };
        }

        public async Task<ServiceResult<ExpandoObject>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var entity = await _userRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false, cancellationToken);

            if (entity == null)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(Entities.User)
                };
            }

            if (entity.Id == ApplicationContext.Instance.CurrentUser.Id)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = ServiceResponseMessage.CANNOT_DELETE_ACTIVE_USER
                };
            }
            
            var lockKey = string.Format(LockKeyConstants.UserLockKey, entity.Id);
            var cacheKey = string.Format(CacheKeyConstants.UserCacheKey, entity.Id);
            
            using (await _lockService.CreateLockAsync(lockKey, cancellationToken))
            {
                await _userRepository.DeleteAsync(entity, cancellationToken); 
                
                await _cacheService.RemoveAsync(cacheKey, cancellationToken);   
            }

            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Deleted(Entities.User, entity.Email)
            };
        }

        public async Task<ServiceResult<PagedList<UserView>>> SearchAsync(FilteryRequest request, CancellationToken cancellationToken)
        {
            var filteryResponse = await _userRepository.Find(p => true).BuildFilteryAsync(new UserFilteryMapping(), request);

            var response = new PagedList<UserView>
            {
                Data = _mapper.Map<List<UserView>>(filteryResponse.Data),
                PageInfo = new Page
                {
                    PageNumber = filteryResponse.PageNumber,
                    PageSize = filteryResponse.PageSize,
                    TotalItemCount = filteryResponse.TotalItemCount
                }
            };
            
            return new ServiceResult<PagedList<UserView>>
            {
                Data = response,
                Status = ResultStatus.Successful
            };
        }

        #endregion


        #region Login Operations

        [PerformanceAspect(2)]
        public async Task<ServiceResult<AccessTokenContract>> GetTokenAsync(GetTokenContractServiceRequest request, CancellationToken cancellationToken)
        {
            var validationResponse = _validationService.Validate(typeof(GetTokenContractServiceRequestValidator), request);

            if (!validationResponse.IsValid)
            {
                return new ServiceResult<AccessTokenContract>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
            }

            var entity = await _userRepository.FindOneAsync(p => p.Email.ToLower().Equals(request.Email.ToLower()) && p.IsDeleted == false, cancellationToken);

            if (entity == null)
            {
                return new ServiceResult<AccessTokenContract>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(request.Email)
                };
            }
 
            if (!BCrypt.Net.BCrypt.Verify(request.Password, entity.Password))
            {
                return new ServiceResult<AccessTokenContract>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(request.Email)
                };
            }

            var token = new JwtManager().GenerateToken(new JwtContract
            {
                Id = entity.Id,
                Name = $"{entity.FirstName} {entity.LastName}",
                Email = entity.Email,
                UserType = entity.Type,
            });

            entity.RefreshToken = token.RefreshToken;
            entity.RefreshTokenExpireDate = token.RefreshTokenExpireDate;

            await _userRepository.UpdateAsync(entity, cancellationToken);

            return new ServiceResult<AccessTokenContract>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Retrieved(),
                Data = token
            };
        }

        [PerformanceAspect(2)]
        public async Task<ServiceResult<AccessTokenContract>> RefreshTokenAsync(RefreshTokenContractServiceRequest request, CancellationToken cancellationToken)
        {
            var validationResponse = _validationService.Validate(typeof(RefreshTokenContractServiceRequestValidator), request);

            if (!validationResponse.IsValid)
            {
                return new ServiceResult<AccessTokenContract>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
            }

            var entity = await _userRepository.FindOneAsync(p =>
                p.RefreshToken == request.Token &&
                p.RefreshTokenExpireDate > DateTime.UtcNow &&
                p.IsDeleted == false,
                cancellationToken);

            if (entity == null)
            {
                return new ServiceResult<AccessTokenContract>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(Entities.User)
                };
            }

            var token = new JwtManager().GenerateToken(new JwtContract
            {
                Id = entity.Id,
                Name = $"{entity.FirstName} {entity.LastName}",
                Email = entity.Email,
                UserType = entity.Type
            });

            entity.RefreshToken = token.RefreshToken;
            entity.RefreshTokenExpireDate = token.RefreshTokenExpireDate;

            await _userRepository.UpdateAsync(entity, cancellationToken);

            return new ServiceResult<AccessTokenContract>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Retrieved(),
                Data = token
            };
        }

        #endregion
    }
}