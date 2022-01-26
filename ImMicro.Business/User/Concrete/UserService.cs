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
using ImMicro.Common.Constans;
using ImMicro.Common.Data.Abstract;
using ImMicro.Common.Mail.Abstract;
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
        private readonly IMailService _mailService;

        public UserService(
            IGenericRepository<Model.User.User> userRepository,
            IMailService mailService)
        {
            _userRepository = userRepository; 
            _mailService = mailService;
        }

        #region CRUD Services
        public async Task<ServiceResult<UserSearchView>> GetAsync(Guid id)
        {
            var user = await _userRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false);

            var (notFoundUserResult, notFoundUserCondition) = ServiceResultHelper.CreateNotFoundResult<UserSearchView>(user, Resource.NotFound(Entities.User));

            return notFoundUserCondition ? notFoundUserResult : ServiceResultHelper.CreateSuccessResult<UserSearchView>(user, Mapper);
        }

        public async Task<ServiceResult<UserSearchView>> GetAsync(string email)
        {
            var user = await _userRepository.FindOneAsync(p => p.Email == email.ToLowerInvariant() && p.IsDeleted == false);

            var (notFoundUserResult, notFoundUserCondition) = ServiceResultHelper.CreateNotFoundResult<UserSearchView>(user, Resource.NotFound(Entities.User));

            return notFoundUserCondition ? notFoundUserResult : ServiceResultHelper.CreateSuccessResult<UserSearchView>(user, Mapper);
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
                EmailVerificationToken = Guid.NewGuid().ToString(),
                Type = Model.User.UserType.Root //TODO: this need to be change as your logic
            };

            entity = await _userRepository.InsertAsync(entity);

            var emailVerificationLink = $"/login/confirm-email/{entity.EmailVerificationToken}";
            var mailRequest = new MailRequest { To = entity.Email, Subject = "Email Confirmation" };
            var mailResponse = await _mailService.SendEmailWithTemplateAsync(mailRequest, MailConstants.VerifyEmailMailTemplate, new { EmailVerificationLink = emailVerificationLink });

            string emailMessage = mailResponse ? Literals.Verify_Email_Message_Success : Literals.Verify_Email_Message_Fail;

            dynamic userWrapper = new ExpandoObject();
            userWrapper.Id = entity.Id;

            return new ServiceResult<ExpandoObject>
            {
                Status = ResultStatus.Successful,
                Message = $"{Resource.Created(Entities.User, entity.Email)}{emailMessage}",
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

            entity.FirstName = request.FirstName.Trim();
            entity.LastName = request.LastName.Trim();
            entity.Email = request.Email.Trim().ToLower();
            entity.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            entity = await _userRepository.UpdateAsync(entity);
            
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

            await _userRepository.DeleteAsync(entity);
            
             return new ServiceResult<ExpandoObject>
             {
                 Status = ResultStatus.Successful,
                 Message = Resource.Deleted(Entities.User, entity.Email)
             };
        }

        public async Task<ServiceResult<PagedList<UserSearchView>>> Search(FilteryRequest request)
        {
            var filteryResponse = await _userRepository.Find(p => true).BuildFilteryAsync(new UserFilteryMapping(), request);
           
            var response = ServiceResultHelper.CreatePagedListResponse<UserSearchView, Model.User.User>(filteryResponse, Mapper);

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

            if (!entity.EmailVerificationTokenIsUsed)
            {
                return new ServiceResult<AccessTokenContract>
                {
                    Status = ResultStatus.BadRequest,
                    Message = Literals.Verify_Email_Message_Success
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
                UserType = entity.Type.GetDisplayName(),
            });

            entity.ResreshToken = token.RefreshToken;
            entity.ResreshTokenExpireDate = token.RefreshTokenExpireDate;

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
                p.ResreshToken == request.Token &&
                p.ResreshTokenExpireDate > DateTime.UtcNow &&
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

            entity.ResreshToken = token.RefreshToken;
            entity.ResreshTokenExpireDate = token.RefreshTokenExpireDate;

            await _userRepository.UpdateAsync(entity);

            return new ServiceResult<AccessTokenContract>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Retrieved(),
                Data = token
            };
        }

        public async Task<ServiceResult<string>> CreateForgotPasswordTokenAsync(ForgotPasswordServiceRequest request)
        {
            var (validationServiceResult, validationServiceCondition) = 
                ServiceResultHelper.CreateValidationResult<ForgotPasswordServiceRequestValidator, string>(request, ValidationService);
            if (validationServiceCondition) return validationServiceResult;

            var user = await _userRepository.FindOneAsync(p => p.Email == request.Email.ToLowerInvariant() && p.IsDeleted == false);
            if (user.ForgotPasswordTokenIsUsed || string.IsNullOrEmpty(user.ForgotPasswordToken))
            {
                user.ForgotPasswordToken = Guid.NewGuid().ToString();
                user.ForgotPasswordTokenIsUsed = false;
                await _userRepository.UpdateAsync(user);
            }

            return ServiceResultHelper.CreateSuccessResult<string>(user.ForgotPasswordToken, Mapper);
        }

        public async Task<ServiceResult<ExpandoObject>> GetForgotPasswordTokenAsync(Guid id)
        {
            var user = await _userRepository.FindOneAsync(p => p.ForgotPasswordToken == id.ToString() && p.IsDeleted == false);

            if (user == null)
            {
                return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.ResourceNotFound, string.Empty);
            }

            if (user.ForgotPasswordTokenIsUsed)
            {
                return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.Failed, Literals.Forgot_Password_Token_Used_Message);
            }

            return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.Successful, string.Empty);
        }

        public async Task<ServiceResult<ExpandoObject>> ResetPasswordAsync(Guid id, ResetPasswordServiceRequest request)
        {
            var (validationServiceResult, validationServiceCondition) = 
                ServiceResultHelper.CreateValidationResult<ResetPasswordServiceRequestValidator, ExpandoObject>(request, ValidationService);
            if (validationServiceCondition) return validationServiceResult;

            var user = await _userRepository.FindOneAsync(p => p.ForgotPasswordToken == id.ToString() && p.IsDeleted == false);

            if (user == null)
            {
                return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.ResourceNotFound, string.Empty);
            }

            if (user.ForgotPasswordTokenIsUsed)
            {
                return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.BadRequest, Literals.Forgot_Password_Token_Used_Message);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.ForgotPasswordTokenIsUsed = true;
            await _userRepository.UpdateAsync(user);

            return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.Successful, string.Empty);
        }

        public async Task<ServiceResult<ExpandoObject>> ChangePasswordAsync(Guid id, ChangePasswordServiceRequest request)
        {
            var (validationServiceResult, validationServiceCondition) = 
                ServiceResultHelper.CreateValidationResult<ChangePasswordServiceRequestValidator, ExpandoObject>(request, ValidationService);
            if (validationServiceCondition) return validationServiceResult;

            var user = await _userRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false);

            if (user == null)
            {
                return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.ResourceNotFound, string.Empty);
            }

            if (user.Id != ApplicationContext.Instance.CurrentUser.Id)
            {
                return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.Forbidden, ServiceResponseMessage.INVALID_REQUEST);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await _userRepository.UpdateAsync(user);

            return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.Successful, string.Empty);
        }

        public async Task<ServiceResult<AccessTokenContract>> ConfirmEmailAsync(Guid id)
        {
            var user = await _userRepository.FindOneAsync(p => p.EmailVerificationToken == id.ToString() && p.IsDeleted == false);

            if (user == null)
            {
                return ServiceResultHelper.CreateResult<AccessTokenContract>(ResultStatus.ResourceNotFound, string.Empty);
            }
            else if (user.EmailVerificationTokenIsUsed)
            {
                return ServiceResultHelper.CreateResult<AccessTokenContract>(ResultStatus.BadRequest, Literals.Verify_Email_Message_Already_Confirmed);
            }

            var token = new JwtManager().GenerateToken(new JwtContract
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                UserType = user.Type.GetDisplayName(),
            });

            user.IsActive = true;
            user.EmailVerificationTokenIsUsed = true;
            user.ResreshToken = token.RefreshToken;
            user.ResreshTokenExpireDate = token.RefreshTokenExpireDate;
            await _userRepository.UpdateAsync(user);

            return new ServiceResult<AccessTokenContract>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Retrieved(),
                Data = token
            };
        }

        public async Task<ServiceResult<ExpandoObject>> ResendConfirmEmailAsync(ResendConfirmEmailServiceRequest request)
        {
            var (validationServiceResult, validationServiceCondition) = 
                ServiceResultHelper.CreateValidationResult<ResendConfirmEmailServiceRequestValidator, ExpandoObject>(request, ValidationService);
            if (validationServiceCondition) return validationServiceResult;

            var user = await _userRepository.FindOneAsync(p => p.Email.ToLower().Equals(request.Email.ToLowerInvariant()) && p.IsDeleted == false);

            if (user == null)
            {
                return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.ResourceNotFound, string.Empty);
            }
            
            if (user.EmailVerificationTokenIsUsed)
            {
                return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.BadRequest, Literals.Verify_Email_Message_Already_Confirmed);
            }

            var emailVerificationLink = $"/login/resend-confirm-email/{user.EmailVerificationToken}";
            var mailRequest = new MailRequest { To = user.Email, Subject = "Email Confirmation" };
            var mailResponse = await _mailService.SendEmailWithTemplateAsync(mailRequest, MailConstants.VerifyEmailMailTemplate, new { EmailVerificationLink = emailVerificationLink });

            var emailMessage = mailResponse ? Literals.Verify_Email_Message_Success : Literals.Verify_Email_Message_Fail;

            return ServiceResultHelper.CreateResult<ExpandoObject>(ResultStatus.Successful, emailMessage);
        }
        #endregion
    }
}