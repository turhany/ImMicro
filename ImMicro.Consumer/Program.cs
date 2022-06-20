using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using System.Runtime.InteropServices;
using ImMicro.Common.Application;
using ImMicro.Common.StartupConfigurations;
using ImMicro.Consumer.Configurations;
using ImMicro.Container.Modules;
using ImMicro.Contract.Mappings.AutoMapper;
using Serilog;

namespace ImMicro.Consumer
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var hostBuilder = Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog((context, conf) => conf.ReadFrom.Configuration(context.Configuration))
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddOptionConfiguration(hostingContext.Configuration);
                    services.AddDatabaseContext(hostingContext.Configuration);
                    services.AddDistributedCacheConfiguration(hostingContext.Configuration);
                    services.AddMassTransitConfigurationForConsumer(hostingContext.Configuration);
                    services.AddAutoMapper(typeof(UserMapping));
                    services.AddHostedService<QueueWorker>();
                })
                .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
                .ConfigureAppConfiguration((hostingContext, config) =>
                { 
                    if (string.IsNullOrWhiteSpace(env))
                    {
                        env = hostingContext.HostingEnvironment.EnvironmentName;
                    }
                    
                    config.AddJsonFile($"appsettings.{env}.json", true, true);
                    config.AddEnvironmentVariables();
                });
             

            hostBuilder.UseConsoleLifetime();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                hostBuilder.UseSystemd();
            else
                hostBuilder.UseWindowsService(); 
            
            var configFile = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{env}.json", optional: false)
                .Build();
            
            ApplicationContext.ConfigureWorkerServiceUser(Guid.Parse(configFile["Application:ServiceUserId"]));
            ApplicationContext.ConfigureThreadPool(configFile);
            
            return hostBuilder;
        }

        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new AspectModule());
        }
    }
}