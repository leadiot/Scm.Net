using Com.Scm.Cache;
using Com.Scm.Cache.Impl;
using Com.Scm.Config;
using Com.Scm.Server.Cache;
using Com.Scm.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm.Server
{
    public static class CacheExtension
    {
        public static void CacheSetup(this IServiceCollection services, EnvConfig envConfig)
        {
            var config = AppUtils.GetConfig<CacheConfig>(CacheConfig.NAME);
            if (config == null)
            {
                config = new CacheConfig();
            }
            config.Prepare(envConfig);

            ICacheService service = new DictionaryService(config);

            services.AddSingleton(service);
        }
    }
}
