using System;
using System.Collections.Generic;
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
using Microsoft.EntityFrameworkCore;

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
            var filteryResponse = await _auditLogRepository.Find(p => true).AsNoTracking().BuildFilteryAsync(new AuditLogFilteryMapping(), request);
           
            var response = new PagedList<AuditLogView>
            {
                Data = Mapper.Map<List<AuditLogView>>(filteryResponse.Data),
                PageInfo = new Page
                {
                    PageNumber = filteryResponse.PageNumber,
                    PageSize = filteryResponse.PageSize,
                    TotalItemCount = filteryResponse.TotalItemCount
                }
            };
            
            return new ServiceResult<PagedList<AuditLogView>>
            {
                Data = response,
                Status = ResultStatus.Successful
            };
        }
        
        public async Task<ServiceResult<AuditLogView>> GetAsync(Guid id)
        {
            var cacheKey = string.Format(CacheKeyConstants.AuditLogCacheKey, id);
            
            var auditLog = await _cacheService.GetOrSetObjectAsync(cacheKey,  async () => await _auditLogRepository.FindOneWithAsNoTrackingAsync(p => p.Id == id && p.IsDeleted == false));

            if (auditLog == null)
            {
                return new ServiceResult<AuditLogView>
                {
                    Status = ResultStatus.ResourceNotFound,
                    Message = Resource.NotFound(Entities.AuditLog)
                };
            }
            
            return new ServiceResult<AuditLogView>
            {
                Status = ResultStatus.Successful,
                Message = Resource.Retrieved(),
                Data = Mapper.Map<AuditLogView>(auditLog)
            };
        }
    }
}