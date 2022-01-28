using System;
using System.Dynamic;
using System.Threading.Tasks;
using Filtery.Extensions;
using Filtery.Models;
using HelpersToolbox.Extensions;
using ImMicro.Business.User.Abstract;
using ImMicro.Business.User.Validator;
using ImMicro.Common.Application;
using ImMicro.Common.Auth;
using ImMicro.Common.Auth.Concrete;
using ImMicro.Common.BaseModels.Mail;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Cache.Abstract;
using ImMicro.Common.Constans;
using ImMicro.Common.Data.Abstract;
using ImMicro.Common.Lock.Abstract;
using ImMicro.Common.Pager;
using ImMicro.Common.Service;
using ImMicro.Contract.App.User;
using ImMicro.Contract.Mappings.Filtery;
using ImMicro.Contract.Service.User;
using ImMicro.Resources.App;
using ImMicro.Resources.Extensions;
using ImMicro.Resources.Model;
using ImMicro.Resources.Service;

namespace ImMicro.Business.User.Concrete
{
    public class UserService : BaseApplicationService, IUserService
    {
        private readonly IGenericRepository<Model.User.User> _userRepository; 
        private readonly ICacheService _cacheService;
        private readonly ILockService _lockService;

        public UserService(
            IGenericRepository<Model.User.User> userRepository,
            ICacheService cacheService, 
            ILockService lockService)
        {
            _userRepository = userRepository; 
            _cacheService = cacheService;
            _lockService = lockService;
        }

        #region CRUD Operations

        public async Task<ServiceResult<UserSearchView>> GetAsync(Guid id)
        {
            var cacheKey = string.Format(CacheKeyConstants.UserCacheKey, id);

            var user = await _cacheService.GetOrSetObjectAsync(cacheKey,
                async () => await _userRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false));

            var (notFoundUserResult, notFoundUserCondition) =
                ServiceResultHelper.CreateNotFoundResult<UserSearchView>(user, Resource.NotFound(Entities.User));

            return notFoundUserCondition
                ? notFoundUserResult
                : ServiceResultHelper.CreateSuccessResult<UserSearchView>(user, Mapper);
        }

        public async Task<ServiceResult<UserSearchView>> GetAsync(string email)
        {
            //TODO: email validator eklenecek

            var cacheKey = string.Format(CacheKeyConstants.UserCacheKey, email);

            var user = await _cacheService.GetOrSetObjectAsync(cacheKey, async () => 
                await _userRepository.FindOneAsync(p => p.Email == email.ToLowerInvariant() && p.IsDeleted == false));

            var (notFoundUserResult, notFoundUserCondition) = ServiceResultHelper.CreateNotFoundResult<UserSearchView>(user, Resource.NotFound(Entities.User));

            return notFoundUserCondition
                ? notFoundUserResult
                : ServiceResultHelper.CreateSuccessResult<UserSearchView>(user, Mapper);
        }

        public async Task<ServiceResult<ExpandoObject>> CreateAsync(CreateUserRequestServiceRequest request)
        {
            var validationResponse = ValidationService.Validate(typeof(CreateUserRequestValidator), request);

            if (!validationResponse.IsValid)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
            }

            if (await _userRepository.AnyAsync(p => p.Email.ToLower().Equals(request.Email.ToLower()) && p.IsDeleted == false))
            {
                return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.BadRequest, Literals.SignUp_Check_Request_Message);
            }

            var entity = new Model.User.User
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Type = Model.User.UserType.Root //TODO: this need to be change as your logic
            };

            entity = await _userRepository.InsertAsync(entity);

            dynamic userWrapper = new ExpandoObject();
            userWrapper.Id = entity.Id;

            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Created(Entities.User, entity.Email),
                Data = userWrapper
            };
        }

        public async Task<ServiceResult<ExpandoObject>> UpdateAsync(UpdateUserRequestServiceRequest request)
        {
            var validationResponse = ValidationService.Validate(typeof(UpdateUserRequestValidator), request);

            if (!validationResponse.IsValid)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
            }

            var entity = await _userRepository.FindOneAsync(p => p.Id == request.Id && p.IsDeleted == false);

            if (entity == null)
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(request.Email)
                };
            }

            if (await _userRepository.AnyAsync(p => p.Id != request.Id && p.Email.ToLower().Equals(request.Email.ToLower()) && p.IsDeleted == false))
            {
                return new ServiceResult<ExpandoObject>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = Resource.Duplicate(entity.Email)
                };
            }

            var lockKey = string.Format(LockKeyConstants.UserLockKey, entity.Id);

            using (await _lockService.CreateLockAsync(lockKey))
            {
                entity.FirstName = request.FirstName.Trim();
                entity.LastName = request.LastName.Trim();
                entity.Email = request.Email.Trim().ToLower();
                entity.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

                entity = await _userRepository.UpdateAsync(entity);
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

        public async Task<ServiceResult<ExpandoObject>> DeleteAsync(Guid id)
        {
            var entity = await _userRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false);

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

            using (await _lockService.CreateLockAsync(lockKey))
            {
                await _userRepository.DeleteAsync(entity);    
            }

            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Deleted(Entities.User, entity.Email)
            };
        }

        public async Task<ServiceResult<PagedList<UserSearchView>>> Search(FilteryRequest request)
        {
            var filteryResponse =
                await _userRepository.Find(p => true).BuildFilteryAsync(new UserFilteryMapping(), request);

            var response =
                ServiceResultHelper.CreatePagedListResponse<UserSearchView, Model.User.User>(filteryResponse, Mapper);

            return ServiceResultHelper.CreateSuccessResult<PagedList<UserSearchView>>(response, Mapper);
        }

        #endregion


        #region Login Operations

        public async Task<ServiceResult<AccessTokenContract>> GetTokenAsync(GetTokenContractServiceRequest request)
        {
            var validationResponse = ValidationService.Validate(typeof(GetTokenContractServiceRequestValidator), request);

            if (!validationResponse.IsValid)
            {
                return new ServiceResult<AccessTokenContract>
                {
                    Status = ResultStatus.InvalidInput,
                    Message = ServiceResponseMessage.INVALID_INPUT_ERROR,
                    ValidationMessages = validationResponse.ErrorMessages
                };
            }

            var entity = await _userRepository.FindOneAsync(p => p.Email.ToLower().Equals(request.Email.ToLower()) && p.IsDeleted == false);

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

            await _userRepository.UpdateAsync(entity);

            return new ServiceResult<AccessTokenContract>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Retrieved(),
                Data = token
            };
        }

        public async Task<ServiceResult<AccessTokenContract>> RefreshTokenAsync(RefreshTokenContractServiceRequest request)
        {
            var validationResponse = ValidationService.Validate(typeof(RefreshTokenContractServiceRequestValidator), request);

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
                p.IsDeleted == false);

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

            await _userRepository.UpdateAsync(entity);

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