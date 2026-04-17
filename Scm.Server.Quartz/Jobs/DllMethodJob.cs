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
                _logger.LogWarning("组别:{Group},名称:{Name},的作业未找到,可能已被移除", trigger.Group, trigger.Name);
                return;
            }
            _logger.LogInformation("组别:{Group},名称:{Name},的作业开始执行,时间:{Time}", trigger.Group, trigger.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            QuarzTaskLogDao tab_Quarz_Tasklog = new QuarzTaskLogDao() { task = taskOptions.names, group = taskOptions.group, begin_time = DateTime.Now };
            if (string.IsNullOrEmpty(taskOptions.dll_uri))
            {
                _logger.LogWarning("组别:{Group},名称:{Name},方法名为空!,时间:{Time}", trigger.Group, trigger.Name, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
                    _logger.LogWarning("组别:{Group},名称:{Name},未找到对应类型:{Type}", trigger.Group, trigger.Name, taskOptions.dll_uri);
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
        }

        public void Dispose()
        {
        }
    }
}
