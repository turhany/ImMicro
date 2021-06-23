using System.Linq;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace ImMicro.Api
{
    /// <summary>
    /// Program
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create Application Host
        /// </summary>
        /// <param name="args">Args</param>
        /// <returns>HostBuilder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog((context, conf) =>
                    conf.ReadFrom.Configuration(context.Configuration)
                        .Enrich.WithCorrelationId()
                        .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger"))))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            var env = hostingContext.HostingEnvironment;
                            config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                            config.AddEnvironmentVariables();
                        });
                });
    }
}