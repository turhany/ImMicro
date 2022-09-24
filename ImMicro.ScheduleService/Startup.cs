using System; 
using Autofac;
using ImMicro.Cache.Redis.StartupConfigurations;
using ImMicro.Common.Application;
using ImMicro.Common.Constans;
using ImMicro.Common.StartupConfigurations;
using ImMicro.Container.Modules;
using ImMicro.Contract.Mappings.AutoMapper;
using ImMicro.Data.EntityFramework.StartupConfigurations;
using ImMicro.Lock.Redis.StartupConfigurations;
using ImMicro.ScheduleService.Configurations;
using ImMicro.ScheduleService.Schedules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 

namespace ImMicro.ScheduleService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkConfiguration(Configuration.GetConnectionString(AppConstants.PostgreSqlConnectionString));
            services.AddDistributedCacheConfiguration(Configuration.GetConnectionString(AppConstants.RedisConnectionString), AppConstants.RedisCacheInstanceName);
            services.AddDistributedLockConfiguration(Configuration, AppConstants.RedLockSettingsOptionName);
            services.AddHangfireConfiguration(Configuration);
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddMassTransitConfiguration(Configuration);
            services.AddAutoMapper(typeof(UserMapping));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseHangfireConfiguration();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            RecurringJobs.SampleJob();
            
            ApplicationContext.ConfigureWorkerServiceUser(Guid.Parse(Configuration["Application:ServiceUserId"]));
            ApplicationContext.ConfigureThreadPool(Configuration);
        }
        
        /// <summary>
        /// Autofac DI Configuration
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());
            builder.RegisterModule(new AspectModule());
        }
    }
}