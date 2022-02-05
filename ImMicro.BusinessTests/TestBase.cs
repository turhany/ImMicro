using System;
using System.Collections.Generic;
using System.Threading;
using Autofac;
using AutoMapper;
using ImMicro.Common.Application;
using ImMicro.Common.Cache.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using ImMicro.Common.Lock.Abstract;
using ImMicro.Container.Modules;
using ImMicro.Contract.Mappings.AutoMapper;
using ImMicro.Data;
using Microsoft.Extensions.Configuration;

namespace ImMicro.BusinessTests
{
    public class TestBase
    {
        protected IContainer Container { get; }
        
        public TestBase()
        {
            //Autofac DI configuration
            var builder = new ContainerBuilder();

            var mockDistributedCache = new Mock<IDistributedCache>();
            mockDistributedCache.Setup(p => p.GetAsync(It.IsAny<string>(), default(CancellationToken))).ReturnsAsync(()=> null);
            
            builder.RegisterInstance(mockDistributedCache.Object).As<IDistributedCache>();
            
            builder.Register(c =>
            {
                var opt = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase("ImMicro_unit_testdb").Options;
                return new DataContext(opt);
            }).InstancePerLifetimeScope();
            
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            
            var mockLock = new Mock<ILockService>();
            builder.RegisterInstance(mockLock.Object).As<ILockService>();
            
            var confLock = new Mock<IConfiguration>();
            builder.RegisterInstance(confLock.Object).As<IConfiguration>();
             
            builder.Register<IMapper>(c =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new UserMapping());
                    cfg.AddProfile(new ProductMapping());
                    cfg.AddProfile(new AuditLogMapping());
                });
                return config.CreateMapper();
            }).SingleInstance();
            
            Container = builder.Build();
            
            ApplicationContext.ConfigureWorkerServiceUser(Guid.NewGuid());
        }
    }
}