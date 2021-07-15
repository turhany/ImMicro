using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImMicro.Common.Data.Abstract;

namespace ImMicro.Business.Services.Abstract
{
    public interface IUserService : IService
    {
        Task ImportDataAsync(MemoryStream memoryStream, CancellationToken cancellationToken);
    }
}