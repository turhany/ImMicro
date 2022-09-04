namespace ImMicro.Cache.Abstract
{
    public interface ICacheService
    {
        Task<T> GetOrSetObjectAsync<T>(string key, Func<T> code, int durationAsMinute, CancellationToken cancellationToken);
        Task<T> GetOrSetObjectAsync<T>(string key, Func<Task<T>> code, int durationAsMinute, CancellationToken cancellationToken);
        Task SetObjectAsync<T>(string key, T value, int durationAsMinute, CancellationToken cancellationToken);
        Task<T> GetObjectAsync<T>(string key, CancellationToken cancellationToken);
        Task<bool> ExistObjectAsync<T>(string key, CancellationToken cancellationToken);

        Task RemoveAsync(string key, CancellationToken cancellationToken);
    }
}