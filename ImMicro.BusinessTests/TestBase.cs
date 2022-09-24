using System; 
using System.Threading;
using Autofac;
using AutoMapper;
using ImMicro.Common.Application;  
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moq; 
using ImMicro.Container.Modules;
using ImMicro.Contract.Mappings.AutoMapper;
using Microsoft.Extensions.Configuration;
using ImMicro.Lock.Abstract;
using ImMicro.Data.EntityFramework;
using System.Diagnostics;
using ImMicro.Common.IoC;
using Microsoft.Extensions.DependencyInjection;
using ImMicro.Model.User;

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
             
            //TODO: need to find solution for run Dapper to InMemory DB
            //builder.Register(c =>
            //{
            //   return new DapperContext("", DapperContext.ConnectionType.PostgreSql);
            //}).InstancePerLifetimeScope();
            
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new AspectModule());

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

            builder.Register(c =>
            {
                return new Stopwatch();
            }).SingleInstance();

            Container = builder.Build();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<Stopwatch>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
             
            GlobalServiceProvider.ServiceProvider = serviceProvider;

            ApplicationContext.ConfigureWorkerServiceUser(Guid.NewGuid());
        }
    }
}