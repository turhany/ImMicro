using ImMicro.Cache.Abstract; 
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json; 

namespace ImMicro.Cache.Redis.Concrete
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetOrSetObjectAsync<T>(string key, Func<T> code, int durationAsMinute, CancellationToken cancellationToken)
        {
            if (await ExistObjectAsync<T>(key, cancellationToken))
            {
                return await GetObjectAsync<T>(key, cancellationToken);
            }

            var result = code.Invoke();

            await SetObjectAsync(key, result, durationAsMinute, cancellationToken);
            return result;
        }

        public async Task<T> GetOrSetObjectAsync<T>(string key, Func<Task<T>> code, int durationAsMinute, CancellationToken cancellationToken)
        {
            if (await ExistObjectAsync<T>(key, cancellationToken))
            {
                return await GetObjectAsync<T>(key, cancellationToken);
            }

            var result = await code.Invoke();

            await SetObjectAsync(key, result, durationAsMinute, cancellationToken);
            return result;
        }

        public async Task SetObjectAsync<T>(string key, T value, int durationAsMinute, CancellationToken cancellationToken)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(durationAsMinute)
            }, cancellationToken);
        }

        public async Task<T> GetObjectAsync<T>(string key, CancellationToken cancellationToken)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            var value = await _distributedCache.GetStringAsync(key, cancellationToken);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> ExistObjectAsync<T>(string key, CancellationToken cancellationToken)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            var value = await _distributedCache.GetStringAsync(key, cancellationToken);
            return value != null;
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }
    }
}