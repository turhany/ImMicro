using System;
using System.Dynamic;
using System.Threading;
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
        Task<ServiceResult<UserView>> GetAsync(Guid id, CancellationToken cancellationToken); 
        Task<ServiceResult<PagedList<UserView>>> SearchAsync(FilteryRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<ExpandoObject>> CreateAsync(CreateUserRequestServiceRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<ExpandoObject>> UpdateAsync(UpdateUserRequestServiceRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<ExpandoObject>> DeleteAsync(Guid id, CancellationToken cancellationToken);


        Task<ServiceResult<AccessTokenContract>> GetTokenAsync(GetTokenContractServiceRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<AccessTokenContract>> RefreshTokenAsync(RefreshTokenContractServiceRequest request, CancellationToken cancellationToken);
    }
}