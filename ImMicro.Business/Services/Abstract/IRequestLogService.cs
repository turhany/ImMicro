using System.Threading.Tasks;
using ImMicro.Common.Data.Abstract;
using ImMicro.Model;

namespace ImMicro.Business.Services.Abstract
{
    public interface IRequestLogService : IService
    {
        Task<bool> SaveAsync(RequestLog entity);
    }
}