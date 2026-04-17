using Com.Scm.Quartz.Dao;
using Com.Scm.Quartz.Service;
using Com.Scm.Utils;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Com.Scm.Quartz.Jobs
{
    /// <summary>
    /// 网络服务
    /// </summary>
    public class ApiClientJob : IJob, IDisposable
    {
        private IQuartzJobService _quartzJobService;
        private IQuartzLogService _quartzLogService;
        private ILogger<ApiClientJob> _logger { get; set; }

        public ApiClientJob(IQuartzJobService quartzService, IQuartzLogService quartzLogService, ILogger<ApiClientJob> logger)
        {
            this._quartzLogService = quartzLogService;
            this._quartzJobService = quartzService;
            this._logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            DateTime dateTime = DateTime.Now;
            string httpMessage = "";
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;

            QuarzTaskJobDao taskOptions = _quartzJobService.GetJobs(a => a.names == trigger.Name && a.group == trigger.Group).Result.FirstOrDefault();
            if (taskOptions == null)
            {

                taskOptions = _quartzJobService.GetJobs(a => a.names == trigger.JobName && a.group == trigger.JobGroup).Result.FirstOrDefault();

            }
            if (taskOptions == null)
            {
                _logger.LogWarning("组别:{Group},名称:{Name},的作业未找到,可能已被移除", trigger.Group, trigger.Name);
                return;
            }
            _logger.LogInformation("组别:{Group},名称:{Name},的作业开始执行,时间:{Time}", trigger.Group, trigger.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            LogUtils.Info($"作业[{taskOptions.names}]开始:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
            QuarzTaskLogDao tab_Quarz_Tasklog = new QuarzTaskLogDao() { task = taskOptions.names, group = taskOptions.group, begin_time = DateTime.Now };
            if (string.IsNullOrEmpty(taskOptions.api_uri) || taskOptions.api_uri == "/")
            {
                _logger.LogWarning("组别:{Group},名称:{Name},参数非法或者异常!,时间:{Time}", trigger.Group, trigger.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return;
            }

            try
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(taskOptions.api_headers))
                {
                    header = taskOptions.api_headers.AsJsonObject<Dictionary<string, string>>();
                }
                Dictionary<string, string> body = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(taskOptions.api_parameter))
                {
                    body = taskOptions.api_parameter.AsJsonObject<Dictionary<string, string>>();
                }

                var method = taskOptions.api_method?.ToLower();
                if (method == "get")
                {
                    httpMessage = await HttpUtils.GetStringAsync(taskOptions.api_uri, body, header);
                }
                else
                {
                    httpMessage = await HttpUtils.PostFormStringAsync(taskOptions.api_uri, body, header);
                }
            }
            catch (Exception ex)
            {
                httpMessage = ex.Message;
                _logger.LogError(ex, "组别:{Group},名称:{Name},作业执行异常", trigger.Group, trigger.Name);
            }

            try
            {
                tab_Quarz_Tasklog.end_time = DateTime.Now;
                tab_Quarz_Tasklog.remark = httpMessage;
                await _quartzLogService.AddLog(tab_Quarz_Tasklog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "组别:{Group},名称:{Name},日志写入失败", trigger.Group, trigger.Name);
            }
            LogUtils.Info(trigger.FullName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + httpMessage);
            return;
        }

        public void Dispose()
        {
        }
    }
}
