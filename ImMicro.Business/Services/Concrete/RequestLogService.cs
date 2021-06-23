using System.Threading.Tasks;
using ImMicro.Business.Services.Abstract;
using ImMicro.Data.Repositories.Abstract;
using ImMicro.Model;

namespace ImMicro.Business.Services.Concrete
{
    public class RequestLogService : IRequestLogService
    {
        private readonly IRequestLogRepository _requestLogRepository;

        public RequestLogService(IRequestLogRepository requestLogRepository)
        {
            _requestLogRepository = requestLogRepository;
        }

        /// <summary>
        /// Save request log
        /// </summary>
        /// <param name="entity">RequestLog Item</param>
        /// <returns>bool</returns>
        public async Task<bool> SaveAsync(RequestLog entity)
        {
            return await _requestLogRepository.AddAsync(entity) != null;
        }
    }
}