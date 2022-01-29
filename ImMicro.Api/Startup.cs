using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using ImMicro.Api.Configurations.Startup;
using ImMicro.Api.Middlewares;
using ImMicro.Common.Application;
using ImMicro.Common.StartupConfigurations;
using ImMicro.Container.Modules;
using ImMicro.Contract.Mappings.AutoMapper;
using Microsoft.AspNetCore.Http;

// ReSharper disable MemberCanBePrivate.Global

namespace ImMicro.Api
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Add Application Services
        /// </summary>
        /// <param name="services">ServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptionConfiguration(Configuration);
            services.AddDatabaseContext(Configuration);
            services.AddIdentityConfigurations(Configuration);
            services.AddLocalizationsConfigurations();
            services.AddDistributedCacheConfiguration(Configuration);
            services.AddCorsConfigurations();
            services.AddControllers().AddNewtonsoftJson();
            services.AddApiVersioningConfigurations();
            services.AddSwaggerConfiguration();
            services.AddHttpContextAccessor();
            services.AddMassTransitConfiguration(Configuration);
            services.AddHealthCheckConfiguration(Configuration);
            services.AddAutoMapper(typeof(UserMapping));
        }

        /// <summary>
        /// Configure Application Services
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="env">Web Host Environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<RequestLogMiddleware>();
            app.UseLocalizationConfiguration();
            app.UseSwaggerConfiguration();
            app.UseHealthCheckConfiguration();
            app.UseSecuritySettings(); 
            app.UseRouting();
            app.UseStaticFiles();
            app.UseCorsConfiguration();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            
            ApplicationContext.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
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
        }
    }
}