using ImMicro.Cache.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace ImMicro.Cache.InMemory.Concrete
{
    public class InMemoryCacheService : ICacheService
    {
        private IMemoryCache _memoryCache;

        public InMemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
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
            var options = new MemoryCacheEntryOptions() { AbsoluteExpiration = DateTime.UtcNow.AddMinutes(durationAsMinute) };
            await Task.Run(() => { _memoryCache.Set(key, value, options); }, cancellationToken);
        }

        public async Task<T> GetObjectAsync<T>(string key, CancellationToken cancellationToken)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            var value = await Task.Run(() => { return _memoryCache.Get<T>(key); }, cancellationToken);
            return value == null ? default(T) : value;
        }

        public async Task<bool> ExistObjectAsync<T>(string key, CancellationToken cancellationToken)
        {
            var value = await GetObjectAsync<T>(key, cancellationToken);
            return value != null;
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            key = $"{key}_{Thread.CurrentThread.CurrentUICulture.Name}";
            await Task.Run(() => { _memoryCache.Remove(key); }, cancellationToken);
        } 
    }
}
