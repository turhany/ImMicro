using Microsoft.AspNetCore.Builder;

namespace ImMicro.Api.Configurations.Startup
{
    /// <summary>
    /// Security setting configuration extension
    /// </summary>
    public static class ConfigureSecuritySettings
    {
        /// <summary>
        /// Use Security setting configuration
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseSecuritySettings(this IApplicationBuilder app)
        {
            //https://docs.nwebsec.com/en/latest/nwebsec/getting-started.html

            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
            app.UseXDownloadOptions();
            app.UseXRobotsTag(options => options.NoIndex().NoFollow());
            app.UseXfo(options => options.SameOrigin());

            return app;
        }
    }
}