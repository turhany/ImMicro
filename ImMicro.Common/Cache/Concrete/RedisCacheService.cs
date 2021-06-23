using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ImMicro.Common.Cache.Abstract;
using ImMicro.Common.Constans; 

namespace ImMicro.Common.Cache.Concrete
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async  Task<T> GetOrSetObjectAsync<T>(string key, Func<T> code, int durationAsMinute = AppConstants.DefaultCacheDuration)
        {
            if (await ExistObjectAsync<T>(key))
            {
                return await GetObjectAsync<T>(key);
            }

            var result = code.Invoke();

            await SetObjectAsync(key, result, durationAsMinute);
            return result;
        }

        public async Task SetObjectAsync<T>(string key, T value, int durationAsMinute = AppConstants.DefaultCacheDuration)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(durationAsMinute)
            });
        }

        public async Task<T> GetObjectAsync<T>(string key)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            var value = await _distributedCache.GetStringAsync(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> ExistObjectAsync<T>(string key)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            var value = await _distributedCache.GetStringAsync(key);
            return value != null;
        }

        public async Task RemoveAsync(string key)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            await _distributedCache.RemoveAsync(key);
        }
    }
}