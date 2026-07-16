using Com.Scm.Config;
using Com.Scm.Dto;
using Com.Scm.Sys;
using Com.Scm.Utils;
using Microsoft.Extensions.Logging;
using Quartz;
using SqlSugar;

namespace Com.Scm.Quartz.Jobs
{
    public class VersionCheckJob : IJob, IDisposable
    {
        private EnvConfig _envConfig;
        private ISqlSugarClient _sqlClient;
        private ILogger<VersionCheckJob> _logger;

        public VersionCheckJob(EnvConfig envConfig, ISqlSugarClient sqlClient, ILogger<VersionCheckJob> logger)
        {
            _envConfig = envConfig;
            _sqlClient = sqlClient;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.LogInformation("[版本检测任务] 开始执行版本检测...");
                await CheckVersionAsync();
                _logger.LogInformation("[版本检测任务] 版本检测执行完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[版本检测任务] 版本检测执行失败");
            }
        }

        private async Task CheckVersionAsync()
        {
            var checkUrl = _envConfig.CheckUpgradeUrl;
            if (string.IsNullOrWhiteSpace(checkUrl))
            {
                _logger.LogWarning("[版本检测任务] 未配置版本检测地址");
                return;
            }

            try
            {
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(30);
                    var json = await http.GetStringAsync(checkUrl);
                    var versionInfo = TextUtils.AsJsonObject<ScmVerInfo>(json);

                    if (versionInfo == null)
                    {
                        _logger.LogWarning("[版本检测任务] 未能获取有效的版本信息");
                        return;
                    }

                    await ProcessNewVersionAsync(versionInfo);
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "[版本检测任务] 获取版本信息失败: {Message}", ex.Message);
            }
        }

        private async Task ProcessNewVersionAsync(ScmVerInfo versionInfo)
        {
            var existingVersion = await _sqlClient.Queryable<ScmUpgradeDao>()
                .Where(u => u.ver_info == versionInfo.GetVer())
                .FirstAsync();

            if (existingVersion != null)
            {
                _logger.LogInformation("[版本检测任务] 版本 {Version} 已存在，无需更新", versionInfo.GetVer());
                return;
            }

            var upgradeInfo = new ScmUpgradeDao
            {
                ver_info = versionInfo.GetVer(),
                ver_code = versionInfo.ver_code,
                ver_date = versionInfo.ver_date,
                phase = versionInfo.phase,
                forced = versionInfo.forced,
                url = versionInfo.url,
                size = versionInfo.size,
                remark = versionInfo.remark,
                ver_min = versionInfo.ver_min,
                ver_max = versionInfo.ver_max
            };

            upgradeInfo.PrepareCreate(0);

            await _sqlClient.Insertable(upgradeInfo).ExecuteCommandAsync();

            _logger.LogInformation("[版本检测任务] 发现新版本 {Version}，已记录到数据库", versionInfo.GetVer());
        }

        public void Dispose()
        {
        }
    }
}