using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Exporty.Abstract;
using Exporty.Extensions;
using Filtery.Extensions;
using Filtery.Models;
using ImMicro.Business.Audit.Abstract;
using ImMicro.Cache.Abstract;
using ImMicro.Cache.Constants;
using ImMicro.Common.BaseModels.Service;
using ImMicro.Common.Constans; 
using ImMicro.Common.Pager;
using ImMicro.Contract.App;
using ImMicro.Contract.Audit;
using ImMicro.Contract.Mappings.Filtery;
using ImMicro.Data.BaseRepositories;
using ImMicro.Model.AuditLog;
using ImMicro.Resources.Extensions;
using ImMicro.Resources.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ImMicro.Business.Audit.Concrete
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IGenericRepository<Model.AuditLog.AuditLog> _auditLogRepository;
        private readonly ICacheService _cacheService;
        private readonly IMapper _mapper;
        private readonly IExporty _exporty;
        private readonly IConfiguration _configuration;

        public AuditLogService(
            IGenericRepository<AuditLog> auditLogRepository, 
            ICacheService cacheService, 
            IMapper mapper, 
            IExporty exporty, 
            IConfiguration configuration)
        {
            _auditLogRepository = auditLogRepository;
            _cacheService = cacheService;
            _mapper = mapper;
            _exporty = exporty;
            _configuration = configuration;
        }

        public async Task<ServiceResult<PagedList<AuditLogView>>> SearchAsync(FilteryRequest request, CancellationToken cancellationToken)
        {
            var filteryResponse = await _auditLogRepository.Find(p => true).AsNoTracking().BuildFilteryAsync(new AuditLogFilteryMapping(), request);
           
            var response = new PagedList<AuditLogView>
            {
                Data = _mapper.Map<List<AuditLogView>>(filteryResponse.Data),
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
        
        public async Task<ServiceResult<string>> ExportAsync(ExportRequest exportRequest, CancellationToken cancellationToken)
        {
            var filteryResponse = await _auditLogRepository.Find(p => true).BuildFilteryAsync(new AuditLogFilteryMapping(), exportRequest.SearchRequest);

            var response = _mapper.Map<List<AuditLogView>>(filteryResponse.Data);
            
            var exportedFileFullPath = _exporty.Export(
                exportRequest.ExportType, 
                response.ToDataTable(), $"{Directory.GetCurrentDirectory()}\\{_configuration[AppConstants.ExportDirectory]}");
            return new ServiceResult<string>
            {
                Data = exportedFileFullPath,
                Status = ResultStatus.Successful
            };
        }

        
        public async Task<ServiceResult<AuditLogView>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var cacheKey = string.Format(CacheKeyConstants.AuditLogCacheKey, id);
            
            var auditLog = await _cacheService.GetOrSetObjectAsync(cacheKey,  
                async () => await _auditLogRepository.FindOneWithAsNoTrackingAsync(p => p.Id == id && p.IsDeleted == false, cancellationToken),
                CacheConstants.DefaultCacheDuration,
                cancellationToken);

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
                Data = _mapper.Map<AuditLogView>(auditLog)
            };
        }
    }
}