using Com.Scm.Sys.Config;

namespace Com.Scm
{
    public interface ICfgService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ConfigDao> GetConfigAsync(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ConfigDao> GetConfigByUserAsync(string key, long userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        Task<ConfigDao> GetConfigByCatAsync(string key, int catId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        Task SaveConfigAsync(ConfigDao config);
    }
}
