using System;
using System.Threading.Tasks;
using Filtery.Models;
using ImMicro.Business.Audit.Abstract;
using ImMicro.Common.BaseModels.Api;
using ImMicro.Common.Pager;
using ImMicro.Contract.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImMicro.Api.Controllers.V1
{
    /// <summary>
    /// Audit Log Controller
    /// </summary>
    public class AuditLogController : BaseController
    {
        private readonly IAuditLogService _auditLogService;
        
        /// <summary>
        /// Audit Log Controller
        /// </summary>
        /// <param name="auditLogService"></param>
        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }
        
        /// <summary>
        /// Search Audit Log
        /// </summary>
        /// <returns></returns>
        [HttpPost("search")]
        //[Authorize(Roles = "Root")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<AuditLogView>))]
        public async Task<ActionResult> Search([FromBody] FilteryRequest request)
        {
            var result = await _auditLogService.Search(request);
            return ApiResponse.CreateResult(result);
        }
        
        /// <summary>
        /// Get Audit Log
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Root")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuditLogView))]
        public async Task<ActionResult> Get(Guid id)
        {
            var result = await _auditLogService.GetAsync(id);
            return ApiResponse.CreateResult(result);
        }
    }
}