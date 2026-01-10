using Com.Scm.Config;
using SqlSugar;

namespace Com.Scm.Service
{
    /// <summary>
    /// App服务
    /// </summary>
    public class AppService : IAppService
    {
        protected EnvConfig _EnvConfig;
        protected ISqlSugarClient _SqlClient;
        protected IResHolder _ResHolder;
        protected Com.Scm.Cache.ICacheService _CacheService;

    }
}
