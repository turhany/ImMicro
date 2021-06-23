using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using ImMicro.Common.Constans;
using ImMicro.Common.Lock.Abstract;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable ConvertToNullCoalescingCompoundAssignment


namespace ImMicro.Common.Lock.Concrete
{
    public class RedisLockService : ILockService
    { 
        static readonly Object RedLockFactory = new Object();
        private static RedLockEndPoint PasswordedServer { get; set; }
        readonly TimeSpan _expiry = TimeSpan.FromSeconds(15);
        private static RedLockFactory _redisLockFactory { get; set; }

        public RedisLockService(IConfiguration configuration)
        { 
            PasswordedServer = new RedLockEndPoint
            {
                EndPoint = new DnsEndPoint(configuration[AppConstants.RedLockHostAddress], int.Parse(configuration[AppConstants.RedLockHostPort])),
                Password = configuration[AppConstants.RedLockHostPassword],
                Ssl =  Boolean.Parse(configuration[AppConstants.RedLockHostSsl])
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