using System;
using System.Threading.Tasks;
using Filtery.Extensions;
using Filtery.Models;
using ImMicro.Business.Audit.Abstract;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Cache.Abstract;
using ImMicro.Common.Constans;
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
        private readonly ICacheService _cacheService;

        public AuditLogService(IGenericRepository<AuditLog> auditLogRepository, ICacheService cacheService)
        {
            _auditLogRepository = auditLogRepository;
            _cacheService = cacheService;
        }

        public async Task<ServiceResult<PagedList<AuditLogView>>> Search(FilteryRequest request)
        {
            var filteryResponse = await _auditLogRepository.Find(p => true).BuildFilteryAsync(new AuditLogFilteryMapping(), request);
           
            var response = ServiceResultHelper.CreatePagedListResponse<AuditLogView, Model.AuditLog.AuditLog>(filteryResponse, Mapper);

            return ServiceResultHelper.CreateSuccessResult<PagedList<AuditLogView>>(response, Mapper);
        }
        
        public async Task<ServiceResult<AuditLogView>> GetAsync(Guid id)
        {
            var cacheKey = string.Format(CacheKeyConstants.AuditLogCacheKey, id);
            
            var auditLog = await _cacheService.GetOrSetObjectAsync(cacheKey,  async () => await _auditLogRepository.FindOneAsync(p => p.Id == id && p.IsDeleted == false));

            var (notFoundAuditLogResult, notFoundAuditLogCondition) = ServiceResultHelper.CreateNotFoundResult<AuditLogView>(auditLog, Resource.NotFound(Entities.AuditLog));

            return notFoundAuditLogCondition ? notFoundAuditLogResult : ServiceResultHelper.CreateSuccessResult<AuditLogView>(auditLog, Mapper);
        }
    }
}