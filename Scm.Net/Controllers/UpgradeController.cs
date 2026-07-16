using Com.Scm.Config;
using Com.Scm.Dto;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Response;
using Com.Scm.Sys;
using Com.Scm.Upgrade.Config;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "v1")]
    public class UpgradeController : ApiController
    {
        private EnvConfig _EnvConfig;
        private ISqlSugarClient _SqlClient;
        private IHostApplicationLifetime _hostLifetime;

        public UpgradeController(EnvConfig envConfig, ISqlSugarClient sqlClient, IHostApplicationLifetime hostLifetime)
        {
            _EnvConfig = envConfig;
            _SqlClient = sqlClient;
            _hostLifetime = hostLifetime;
        }

        /// <summary>
        /// 获取新版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ScmVerInfo> GetAsync()
        {
            var latestUpgrade = await _SqlClient.Queryable<ScmUpgradeDao>()
                .FirstAsync();

            if (latestUpgrade == null)
            {
                return null;
            }

            return MapToVerInfo(latestUpgrade);
        }

        /// <summary>
        /// 检测更新
        /// </summary>
        /// <returns></returns>
        [HttpGet("check")]
        public async Task<ScmVerInfo> CheckUpdateAsync()
        {
            var url = _EnvConfig.CheckUpgradeUrl;
            if (string.IsNullOrWhiteSpace(url))
            {
                //url = "http://api.c-scm.com/api/Ver/{0}_{1}_{2}";
                url = "http://www.c-scm.com/api/ScmSysApp/Ver?code={0}&client={1}&build={2}";
            }
            url = string.Format(url, ScmServerEnv.APP_CODE, ScmClientTypeEnum.Web, ScmServerEnv.BUILD);

            var response = await HttpUtils.GetObjectAsync<ScmApiDataResponse<ScmVerInfo>>(url);
            if (response == null)
            {
                return null;
            }

            if (!response.IsSuccess())
            {
                throw new BusinessException(response.GetMessage());
            }

            var dto = response.Data;
            var dao = await _SqlClient.Queryable<ScmUpgradeDao>()
                .Where(a => a.build == dto.build)
                .FirstAsync();
            if (dao == null)
            {
                dao = dto.Adapt<ScmUpgradeDao>();
                await _SqlClient.Insertable(dao).ExecuteCommandAsync();
            }

            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("download")]
        public async Task<bool> DownloadUpdate(long id)
        {
            var dao = await _SqlClient.Queryable<ScmUpgradeDao>()
                .FirstAsync(a => a.id == id);
            if (dao == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(dao.url))
            {
                throw new BusinessException("下载路径为空！");
            }

            if (dao.handle == ScmHandleEnum.Doing || dao.result == ScmResultEnum.Success)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(dao.file))
            {
                dao.file = _EnvConfig.GetTempPath($"upgrade_{dao.ver_code}.zip");
            }
            dao.handle = ScmHandleEnum.Doing;

            await HttpUtils.DownloadFileAsync(dao.url, dao.file);
            await _SqlClient.Updateable(dao).ExecuteCommandAsync();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("upgrade")]
        public async Task<bool> UpgradeAsync(long id)
        {
            var dao = await _SqlClient.Queryable<ScmUpgradeDao>()
                .FirstAsync(a => a.id == id);
            if (dao == null)
            {
                throw new BusinessException("指定的版本不存在！");
            }

            if (dao.handle == ScmHandleEnum.Doing)
            {
                throw new BusinessException("升级文件下载中，请稍后……");
            }
            if (dao.result != ScmResultEnum.Success)
            {
                throw new BusinessException("升级文件下载失败，请重新下载……");
            }

            if (!System.IO.File.Exists(dao.file))
            {
                throw new BusinessException("升级文件不存在，请重新下载……");
            }

            var installPath = AppContext.BaseDirectory;

            var upgradeFile = _EnvConfig.UpgradeFilePath ?? ScmServerEnv.UpgradeFilePath;
            upgradeFile = System.IO.Path.Combine(installPath, upgradeFile);
            if (!System.IO.File.Exists(upgradeFile))
            {
                throw new BusinessException("升级程序不存在，无法执行升级！");
            }

            var launchFile = Path.Combine("dotnet Scm.Net.dll");

            SaveUpgradeJson(installPath, launchFile, dao);

            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(upgradeFile)
            {
                //CreateNoWindow = true,
                UseShellExecute = true,
                WorkingDirectory = System.IO.Path.GetDirectoryName(upgradeFile)
            });

            _hostLifetime.StopApplication();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("new")]
        public async Task<bool> HasNewVersionAsync()
        {
            var latestVersion = await CheckUpdateAsync();
            if (latestVersion == null)
            {
                return false;
            }

            return latestVersion.IsNewer(ScmServerEnv.BUILD);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("ver")]
        public async Task<List<ScmVerInfo>> GetVersionHistoryAsync()
        {
            var upgrades = await _SqlClient.Queryable<ScmUpgradeDao>()
                .OrderByDescending(u => u.id)
                .ToListAsync();

            return upgrades.Select(MapToVerInfo).ToList();
        }

        private ScmVerInfo MapToVerInfo(ScmUpgradeDao upgradeInfo)
        {
            return new ScmVerInfo
            {
                major = upgradeInfo.major,
                minor = upgradeInfo.minor,
                patch = upgradeInfo.patch,
                ver_info = upgradeInfo.ver_info,
                ver_code = upgradeInfo.ver_code,
                ver_date = upgradeInfo.ver_date,
                phase = upgradeInfo.phase,
                forced = upgradeInfo.forced,
                url = upgradeInfo.url,
                size = upgradeInfo.size,
                hash = upgradeInfo.hash,
                remark = upgradeInfo.remark,
                ver_min = upgradeInfo.ver_min,
                ver_max = upgradeInfo.ver_max
            };
        }

        private void SaveUpgradeJson(string installPath, string launchFile, ScmUpgradeDao verInfo)
        {
            var config = new UpgradeConfig();
            config.Title = "Scm.Net";
            config.AppInfo = "";
            config.VerInfo = verInfo.remark;
            config.OldVersion = ScmServerEnv.VER_INFO;
            config.NewVersion = verInfo.ver_info;
            config.AutoStart = true;
            config.AutoClose = true;
            config.LogToFile = true;

            var backupPath = Path.Combine(installPath, "backup");
            var tempPath = Path.Combine(installPath, "temp");

            var dataFile = Path.Combine(installPath, "data", "scm.db");
            var time = DateTime.Now.ToString("yyyyMMddHHmmss");
            var backupFile = Path.Combine(backupPath, "data", $"scm-{time}.db");

            // 1、创建备份目录
            var stepConfig = StepConfig.NewCreateDirStep("创建备份目录", backupPath);
            config.AddStep(stepConfig);

            // 2、创建临时目录
            stepConfig = StepConfig.NewCreateDirStep("创建临时目录", tempPath);
            config.AddStep(stepConfig);

            // 3、升级文件下载
            //var downloadFile = Path.Combine(tempPath, "upgrade.zip");
            //stepConfig = StepConfig.NewDownloadStep("升级文件下载", verInfo.url, downloadFile);
            //config.AddStep(stepConfig);

            // 4、数据备份
            stepConfig = StepConfig.NewMoveDocStep("数据备份", dataFile, backupFile);
            stepConfig.ContinueOnError = true;
            config.AddStep(stepConfig);

            // 5、升级包解压
            stepConfig = StepConfig.NewUnzipStep("升级包解压", verInfo.file, tempPath);
            config.AddStep(stepConfig);

            // 6、应用更新
            stepConfig = StepConfig.NewMoveDirStep("应用更新", Path.Combine(tempPath, "Scm.Web"), installPath);
            config.AddStep(stepConfig);

            // 7、数据恢复
            stepConfig = StepConfig.NewCopyDocStep("数据恢复", backupFile, dataFile);
            stepConfig.ContinueOnError = true;
            config.AddStep(stepConfig);

            // 8、配置恢复
            //stepConfig = StepConfig.NewCopyDocStep("配置恢复", Path.Combine(installPath, launchFile), null);
            //config.AddStep(stepConfig);

            // 9、启用应用
            stepConfig = StepConfig.NewLaunchStep("启用应用", launchFile, "", installPath);
            config.AddStep(stepConfig);

            // 10、临时文件清理
            stepConfig = StepConfig.NewDeleteDirStep("临时文件清理", tempPath);
            config.AddStep(stepConfig);

            var jsonFile = Path.Combine(installPath, _EnvConfig.UpgradeJsonPath ?? ScmServerEnv.UpgradeJsonPath);
            var jsonDir = Path.GetDirectoryName(jsonFile);
            if (!Directory.Exists(jsonDir))
            {
                Directory.CreateDirectory(jsonDir);
            }

            var json = config.ToJsonString();
            System.IO.File.WriteAllText(jsonFile, json);
        }
    }
}