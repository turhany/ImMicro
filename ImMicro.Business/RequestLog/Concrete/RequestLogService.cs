using System.Threading;
using System.Threading.Tasks;
using ImMicro.Business.RequestLog.Abstract; 
using ImMicro.Data.BaseRepositories;

namespace ImMicro.Business.RequestLog.Concrete
{
    public class RequestLogService : IRequestLogService
    {
        private readonly IGenericRepository<Model.RequestLog.RequestLog> _requestLogRepository;

        public RequestLogService(IGenericRepository<Model.RequestLog.RequestLog> requestLogRepository)
        {
            _requestLogRepository = requestLogRepository;
        }

        /// <summary>
        /// Save request log
        /// </summary>
        /// <param name="entity">RequestLog Item</param>
        /// <returns>bool</returns>
        public async Task<bool> SaveAsync(Model.RequestLog.RequestLog entity, CancellationToken cancellationToken)
        {
            return await _requestLogRepository.InsertAsync(entity, cancellationToken) != null;
        }
    }
}