using Com.Scm.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm.Extensions
{
    /// <summary>
    /// 国际化支持
    /// </summary>
    public static class I18NExtension
    {
        public static void I18NSetup(this IServiceCollection services, EnvConfig config)
        {
            if (config == null || config.SupportedCultures == null || config.SupportedCultures.Length < 1)
            {
                return;
            }

            services.AddLocalization(options => options.ResourcesPath = config.ResourcesPath);
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture(config.DefaultCulture)
                    .AddSupportedCultures(config.SupportedCultures)
                    .AddSupportedUICultures(config.SupportedCultures);
            });
        }

        public static void UseI18N(this IApplicationBuilder app)
        {
            app.UseRequestLocalization();
        }
    }
}
