using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using ImMicro.Common.Lock.Abstract;
using ImMicro.Common.Options;
using Microsoft.Extensions.Options;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ConvertToNullCoalescingCompoundAssignment


namespace ImMicro.Common.Lock.Concrete
{
    public class RedisLockService : ILockService
    { 
        static readonly Object RedLockFactory = new();
        private static RedLockEndPoint PasswordedServer { get; set; }
        readonly TimeSpan _expiry = TimeSpan.FromSeconds(15);
        private static RedLockFactory _redisLockFactory { get; set; }

        public RedisLockService(IOptions<RedLockOption> redLockConfig)
        { 
            PasswordedServer = new RedLockEndPoint
            {
                EndPoint = new DnsEndPoint(redLockConfig.Value.HostAddress, int.Parse(redLockConfig.Value.HostPort)),
                Password = redLockConfig.Value.HostPassword,
                Ssl =  redLockConfig.Value.HostSsl
            };
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

        public async Task<IDisposable> CreateLockAsync(string key)
        {
            int i = 0;
            IRedLock locked = null;
            while (i < 10)
            {
                i++;
                var resource = key;

                locked = await RedisLockFactory.CreateLockAsync(resource, _expiry);

                if (locked.IsAcquired)
                {
                    return locked;
                }
            }
            return locked;
        }
    }
}