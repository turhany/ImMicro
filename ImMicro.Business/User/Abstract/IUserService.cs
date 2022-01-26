using System;
using System.Dynamic;
using System.Threading.Tasks;
using Filtery.Models;
using ImMicro.Common.Auth.Concrete;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Data.Abstract;
using ImMicro.Common.Pager;
using ImMicro.Contract.App.User;
using ImMicro.Contract.Service.User;

namespace ImMicro.Business.User.Abstract
{
    public interface IUserService : IService
    {
        Task<ServiceResult<UserSearchView>> GetAsync(Guid id);
        Task<ServiceResult<UserSearchView>> GetAsync(string email);
        Task<ServiceResult<PagedList<UserSearchView>>> Search(FilteryRequest request);
        Task<ServiceResult<ExpandoObject>> CreateAsync(CreateUserRequestServiceRequest request);
        Task<ServiceResult<ExpandoObject>> UpdateAsync(UpdateUserRequestServiceRequest request);
        Task<ServiceResult<ExpandoObject>> DeleteAsync(Guid id);


        Task<ServiceResult<AccessTokenContract>> GetTokenAsync(GetTokenContractServiceRequest request);
        Task<ServiceResult<AccessTokenContract>> RefreshTokenAsync(RefreshTokenContractServiceRequest request);
        Task<ServiceResult<string>> CreateForgotPasswordTokenAsync(ForgotPasswordServiceRequest request);
        Task<ServiceResult<ExpandoObject>> GetForgotPasswordTokenAsync(Guid id);
        Task<ServiceResult<ExpandoObject>> ResetPasswordAsync(Guid id, ResetPasswordServiceRequest request);
        Task<ServiceResult<ExpandoObject>> ChangePasswordAsync(Guid id, ChangePasswordServiceRequest request);
        Task<ServiceResult<AccessTokenContract>> ConfirmEmailAsync(Guid id);
        Task<ServiceResult<ExpandoObject>> ResendConfirmEmailAsync(ResendConfirmEmailServiceRequest request);
    }
}