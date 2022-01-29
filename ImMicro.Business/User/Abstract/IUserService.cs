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
        Task<ServiceResult<UserView>> GetAsync(Guid id); 
        Task<ServiceResult<PagedList<UserView>>> SearchAsync(FilteryRequest request);
        Task<ServiceResult<ExpandoObject>> CreateAsync(CreateUserRequestServiceRequest request);
        Task<ServiceResult<ExpandoObject>> UpdateAsync(UpdateUserRequestServiceRequest request);
        Task<ServiceResult<ExpandoObject>> DeleteAsync(Guid id);


        Task<ServiceResult<AccessTokenContract>> GetTokenAsync(GetTokenContractServiceRequest request);
        Task<ServiceResult<AccessTokenContract>> RefreshTokenAsync(RefreshTokenContractServiceRequest request);
    }
}