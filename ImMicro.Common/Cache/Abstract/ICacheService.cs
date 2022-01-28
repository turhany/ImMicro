using System;
using System.Threading.Tasks;
using ImMicro.Common.Constans;

namespace ImMicro.Common.Cache.Abstract
{
    public interface ICacheService
    {
        Task<T> GetOrSetObjectAsync<T>(string key, Func<T> code, int durationAsMinute = AppConstants.DefaultCacheDuration);
        Task<T> GetOrSetObjectAsync<T>(string key, Func<Task<T>> code, int durationAsMinute = AppConstants.DefaultCacheDuration);
        Task SetObjectAsync<T>(string key, T value, int durationAsMinute = AppConstants.DefaultCacheDuration);
        Task<T> GetObjectAsync<T>(string key);
        Task<bool> ExistObjectAsync<T>(string key);

        Task RemoveAsync(string key);
    }
}