using Com.Scm.Sys.Config;

namespace Com.Scm
{
    public interface ICfgService
    {
        Task<ConfigDao> GetConfigAsync(string key);

        Task SaveConfigAsync(ConfigDao config);
    }
}
