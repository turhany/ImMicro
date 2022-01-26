using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ImMicro.Api.Configurations.Startup
{
    /// <summary>
    /// Configure Localization
    /// </summary>
    public static class ConfigureLocalization
    {
        /// <summary>
        /// Add Localizations Configurations
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLocalizationsConfigurations(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("tr-TR")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
               
                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
                {
                    CultureInfo resourceCulture = supportedCultures.First();
                    
                    var requestLanguages = context.Request.GetTypedHeaders().AcceptLanguage;
                    
                    foreach (var language in requestLanguages)
                    {
                        var selectedCulture = supportedCultures.FirstOrDefault(p => string.Equals(p.Name, language.Value.Value, StringComparison.CurrentCultureIgnoreCase));
                        if (selectedCulture != null)
                        {
                            resourceCulture = selectedCulture;
                            break;
                        }
                    }
                    
                    return Task.FromResult(new ProviderCultureResult(resourceCulture.Name, resourceCulture.Name));
                }));
                
            });
            
            return services;
        }

        /// <summary>
        /// Use Localization Configuration
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLocalizationConfiguration(this IApplicationBuilder app)
        {
            var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizeOptions.Value);

            return app;
        }
    }
}