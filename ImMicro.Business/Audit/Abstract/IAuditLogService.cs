using System;
using System.Threading;
using System.Threading.Tasks;
using Filtery.Models;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Data.Abstract;
using ImMicro.Common.Pager;
using ImMicro.Contract.App;
using ImMicro.Contract.Audit;

namespace ImMicro.Business.Audit.Abstract
{
    public interface IAuditLogService : IService
    {
        Task<ServiceResult<PagedList<AuditLogView>>> SearchAsync(FilteryRequest request, CancellationToken cancellationToken);
        Task<ServiceResult<string>> ExportAsync(ExportRequest exportRequest, CancellationToken cancellationToken);
        Task<ServiceResult<AuditLogView>> GetAsync(Guid id, CancellationToken cancellationToken);
    }
}