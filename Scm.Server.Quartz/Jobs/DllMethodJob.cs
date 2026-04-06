using Com.Scm.Quartz.Dao;
using Com.Scm.Quartz.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Com.Scm.Quartz.Jobs
{
    /// <summary>
    /// 本地服务
    /// </summary>
    public class DllMethodJob : IJob, IDisposable
    {
        private IQuartzJobService _quartzService;
        private IQuartzLogService _quartzLogService;
        private IServiceProvider _serviceProvider;
        private ILogger<DllMethodJob> _logger { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="quartzService"></param>
        public DllMethodJob(IServiceProvider serviceProvider, IQuartzJobService quartzService, IQuartzLogService quartzLogService, ILogger<DllMethodJob> logger)
        {
            this._quartzLogService = quartzLogService;
            this._quartzService = quartzService;
            this._logger = logger;
            this._serviceProvider = serviceProvider;
            //serviceProvider.GetService()
        }

        public async Task Execute(IJobExecutionContext context)
        {
            string httpMessage = "";
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;

            QuarzTaskJobDao taskOptions = (await _quartzService.GetJobs(a => a.names == trigger.Name && a.group == trigger.Group)).FirstOrDefault();
            if (taskOptions == null)
            {
                taskOptions = (await _quartzService.GetJobs(a => a.names == trigger.JobName && a.group == trigger.JobGroup)).FirstOrDefault();
            }

            if (taskOptions == null)
            {
                _logger.LogError($"组别:{trigger.Group},名称:{trigger.Name},的作业未找到,可能已被移除");
                // FileHelper.WriteFile(FileQuartz.LogPath + trigger.Group, $"{trigger.Name}.txt", "未到找作业或可能被移除", true);
                return;
            }
            _logger.LogError($"组别:{trigger.Group},名称:{trigger.Name},的作业开始执行,时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
            //Console.WriteLine($"作业[{taskOptions.TaskName}]开始:{ DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
            QuarzTaskLogDao tab_Quarz_Tasklog = new QuarzTaskLogDao() { task = taskOptions.names, group = taskOptions.group, begin_time = DateTime.Now };
            if (string.IsNullOrEmpty(taskOptions.dll_uri))
            {
                _logger.LogError($"组别:{trigger.Group},名称:{trigger.Name},方法名为空!,时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
                //FileHelper.WriteFile(FileQuartz.LogPath + trigger.Group, $"{trigger.Name}.txt", $"{ DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}未配置url,", true);
                return;
            }

            try
            {
                var services = _serviceProvider.GetServices<ICustomJob>();
                var service = services.Where(a => a.GetType().FullName == taskOptions.dll_uri).FirstOrDefault();
                if (service != null)
                {
                    httpMessage = service.ExecuteService(taskOptions.dll_parameter);
                }
                else
                {
                    httpMessage = "未找到对应类型,请检查是否注入!";
                }
            }
            catch (Exception ex)
            {
                httpMessage = ex.Message;
            }

            try
            {
                //string logContent = $"{(string.IsNullOrEmpty(httpMessage) ? "OK" : httpMessage)}\r\n";
                tab_Quarz_Tasklog.end_time = DateTime.Now;
                tab_Quarz_Tasklog.remark = httpMessage;
                await _quartzLogService.AddLog(tab_Quarz_Tasklog);
            }
            catch (Exception)
            {
                // ignored
            }
            //Console.WriteLine(trigger.FullName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + " " + httpMessage);
        }

        public void Dispose()
        {
        }
    }
}
