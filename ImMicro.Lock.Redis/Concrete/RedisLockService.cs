using System.Net;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration; 
using Microsoft.Extensions.Options;
using ImMicro.Lock.Abstract;
using ImMicro.Lock.Redis.Options;
using ImMicro.Lock.Exceptions; 

namespace ImMicro.Lock.Redis.Concrete
{
    public class RedisLockService : ILockService
    {
        private static readonly object RedLockFactory = new();
        private static RedLockEndPoint PasswordedServer { get; set; }
        private static RedLockFactory _redisLockFactory { get; set; }

        private static TimeSpan ExpiryTime;
        private static TimeSpan WaitTime;
        private static TimeSpan RetryTime;

        public RedisLockService(IOptions<RedLockOption> redLockConfig)
        {
            PasswordedServer = new RedLockEndPoint
            {
                EndPoint = new DnsEndPoint(redLockConfig.Value.HostAddress, int.Parse(redLockConfig.Value.HostPort)),
                Password = redLockConfig.Value.HostPassword,
                Ssl = redLockConfig.Value.HostSsl
            };

            ExpiryTime = TimeSpan.FromSeconds(redLockConfig.Value.ExpireTimeAsSecond);
            WaitTime = TimeSpan.FromSeconds(redLockConfig.Value.WaitTimeAsSecond);
            RetryTime = TimeSpan.FromSeconds(redLockConfig.Value.RetryTimeAsSecond);
        }

        static RedLockFactory RedisLockFactory
        {
            get
            {
                lock (RedLockFactory)
                {
                    return _redisLockFactory ?? (_redisLockFactory = new RedLockFactory(new RedLockConfiguration(new List<RedLockEndPoint> { PasswordedServer })));
                }
            }
        }

        public async Task<IDisposable> CreateLockAsync(string key, CancellationToken cancellationToken)
        {
            IRedLock locked;
            try
            {
                locked = await RedisLockFactory.CreateLockAsync(key, ExpiryTime, WaitTime, RetryTime ,cancellationToken);

                if (locked.IsAcquired)
                {
                    return locked;
                }
                else
                {
                    new AcquireLockException("The lock wasn't acquired");
                }
            }
            catch
            {
                throw;
            }

            return locked;
        }
    }
}