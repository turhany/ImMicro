using System;
using System.Threading.Tasks;

namespace ImMicro.Common.Lock.Abstract
{
    public interface ILockService
    {
        Task<IDisposable> CreateLockAsync(string key);
    }
}