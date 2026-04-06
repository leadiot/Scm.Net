using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Sys.Config;
using Com.Scm.Utils;

namespace Com.Scm.Service
{
    /// <summary>
    /// 系统配置服务
    /// </summary>
    public class ScmCfgService : ICfgService
    {
        private readonly SugarRepository<ConfigDao> _configRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configRepository"></param>
        public ScmCfgService(SugarRepository<ConfigDao> configRepository)
        {
            _configRepository = configRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ConfigDao GetConfig(string key)
        {
            return _configRepository.GetFirst(a => a.key == key && a.row_status == ScmRowStatusEnum.Enabled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<ConfigDao> GetConfigAsync(string key)
        {
            return await _configRepository.GetFirstAsync(a => a.key == key && a.row_status == ScmRowStatusEnum.Enabled);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task SaveConfigAsync(ConfigDao config)
        {
            var dao = await _configRepository.GetFirstAsync(a => a.key == config.key);
            if (dao != null)
            {
                CommonUtils.Adapt(config, dao);
                await _configRepository.UpdateAsync(config);
                return;
            }

            await _configRepository.InsertAsync(config);
        }
    }
}
