using System.Threading;
using System.Threading.Tasks;
using ImMicro.Common.Data.Abstract;

namespace ImMicro.Business.RequestLog.Abstract
{
    public interface IRequestLogService : IService
    {
        Task<bool> SaveAsync(Model.RequestLog.RequestLog entity, CancellationToken cancellationToken);
    }
}