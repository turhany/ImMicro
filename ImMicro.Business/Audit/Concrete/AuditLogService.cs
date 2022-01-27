using System;
using System.Threading.Tasks;
using Filtery.Extensions;
using Filtery.Models;
using ImMicro.Business.Audit.Abstract;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Data.Abstract;
using ImMicro.Common.Pager;
using ImMicro.Common.Service;
using ImMicro.Contract.Audit;
using ImMicro.Contract.Mappings.Filtery;
using ImMicro.Model.AuditLog;
using ImMicro.Resources.Extensions;
using ImMicro.Resources.Model;

namespace ImMicro.Business.Audit.Concrete
{
    public class AuditLogService : BaseApplicationService, IAuditLogService
    {
        private readonly IGenericRepository<Model.AuditLog.AuditLog> _auditLogRepository;

        public AuditLogService(IGenericRepository<AuditLog> auditLogRepository)
        {
            _auditLogRepository = auditLogRepository;
        }

        public async Task<ServiceResult<PagedList<AuditLogView>>> Search(FilteryRequest request)
        {
            var filteryResponse = await _auditLogRepository.Find(p => true).BuildFilteryAsync(new AuditLogFilteryMapping(), request);
           
            var response = ServiceResultHelper.CreatePagedListResponse<AuditLogView, Model.AuditLog.AuditLog>(filteryResponse, Mapper);

            return ServiceResultHelper.CreateSuccessResult<PagedList<AuditLogView>>(response, Mapper);
        }
        
        public async Task<ServiceResult<AuditLogView>> GetAsync(Guid id)
        {
            var user = await _auditLogRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false);

            var (notFoundUserResult, notFoundUserCondition) = ServiceResultHelper.CreateNotFoundResult<AuditLogView>(user, Resource.NotFound(Entities.AuditLog));

            return notFoundUserCondition ? notFoundUserResult : ServiceResultHelper.CreateSuccessResult<AuditLogView>(user, Mapper);
        }
    }
}