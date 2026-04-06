using Com.Scm.Quartz.Dao;
using SqlSugar;

namespace Com.Scm.Quartz.Service.Db
{
    /// <summary>
    /// 基于数据库的日志管理服务
    /// </summary>
    public class DbQuartzLogService : IQuartzLogService
    {
        private ISqlSugarClient _quarzContext;
        public DbQuartzLogService(ISqlSugarClient quarzContext)
        {
            _quarzContext = quarzContext;
        }

        public async Task<JobResult> AddLog(QuarzTaskLogDao tab_Quarz_Tasklog)
        {
            var result = new JobResult { status = false, message = "" };

            var date = await _quarzContext.Insertable(tab_Quarz_Tasklog).ExecuteCommandAsync();
            if (date > 0)
            {
                result.status = true;
                result.message = "数据库添加成功!";
            }

            return result;
        }

        public async Task<QuarzTaskLogDao> GetLastlog(string taskName, string groupName)
        {
            var data = await _quarzContext.Queryable<QuarzTaskLogDao>()
                .Where(a => a.task == taskName && a.group == groupName)
                .OrderBy(a => a.id, OrderByType.Desc)
                .FirstAsync();
            return data;
        }

        public async Task<ResultData<QuarzTaskLogDao>> GetLogs(string taskName, string groupName, int page, int pageSize = 100)
        {
            int total = await _quarzContext.Queryable<QuarzTaskLogDao>()
                .Where(a => a.task == taskName && a.group == groupName)
                .CountAsync();

            var pagem = page - 1;
            var data = await _quarzContext.Queryable<QuarzTaskLogDao>()
                .Where(a => a.task == taskName && a.group == groupName)
                .OrderBy(a => a.id, OrderByType.Desc)
                .Skip(pagem * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ResultData<QuarzTaskLogDao> resultData = new ResultData<QuarzTaskLogDao>() { total = total, data = data };
            return resultData;
        }
    }
}
