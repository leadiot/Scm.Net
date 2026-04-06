using Com.Scm.Quartz.Dao;

namespace Com.Scm.Quartz.Service.Df
{
    /// <summary>
    /// 基于文件的日志管理服务
    /// </summary>
    public class DfQuartzLogService : IQuartzLogService
    {
        private QuartzFileHelper _Helper;
        public DfQuartzLogService(QuartzFileHelper helper)
        {
            _Helper = helper;
        }

        public Task<JobResult> AddLog(QuarzTaskLogDao tab_Quarz_Tasklog)
        {
            return Task.Run(() =>
            {
                try
                {
                    _Helper.WriteJobLogs(tab_Quarz_Tasklog);
                    return new JobResult { message = "日志数据保存成功!", status = true };
                }
                catch (Exception)
                {

                    return new JobResult { message = "日志数据保存失败!", status = false };
                }
            });
        }

        public Task<QuarzTaskLogDao> GetLastlog(string taskName, string groupName)
        {
            return Task.Run(() =>
            {

                var list = _Helper.GetJobsLog();
                var date = list.Where(a => a.task == taskName && a.group == groupName).OrderByDescending(a => a.begin_time).FirstOrDefault();
                return date;

            });
        }

        public Task<ResultData<QuarzTaskLogDao>> GetLogs(string taskName, string groupName, int page, int pageSize = 100)
        {
            return Task.Run(() =>
            {

                var list = _Helper.GetJobsLog();
                int total = list.Where(a => a.task == taskName
            && a.group == groupName).Count();
                var date = list.Where(a => a.task == taskName && a.group == groupName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                ResultData<QuarzTaskLogDao> resultData = new ResultData<QuarzTaskLogDao>() { total = total, data = date };
                return resultData;

            });
        }
    }
}
