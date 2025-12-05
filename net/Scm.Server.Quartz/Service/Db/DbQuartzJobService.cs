using Com.Scm.Quartz.Dao;
using SqlSugar;
using System.Linq.Expressions;

namespace Com.Scm.Quartz.Service.Db
{
    /// <summary>
    /// 基于数据库的任务管理服务
    /// </summary>
    public class DbQuartzJobService : IQuartzJobService
    {
        private ISqlSugarClient _Client;

        public DbQuartzJobService(ISqlSugarClient client)
        {
            _Client = client;
        }

        public async Task<JobResult> AddJob(QuarzTaskJobDao model)
        {
            var date = await _Client.Insertable(model).ExecuteCommandAsync();
            if (date > 0)
            {
                JobResult.Success("数据库添加成功！");
            }

            return JobResult.Failure("数据加添加异常！");
        }

        public async Task<List<QuarzTaskJobDao>> GetJobs(Expression<Func<QuarzTaskJobDao, bool>> where = null)
        {
            return await _Client.Queryable<QuarzTaskJobDao>().Where(where).ToListAsync();
        }

        public async Task<JobResult> Remove(QuarzTaskJobDao model)
        {
            var result = new JobResult { status = false, message = "" };

            var date = await _Client.Deleteable<QuarzTaskJobDao>().Where(m => m.id == model.id).ExecuteCommandAsync();
            if (date > 0)
            {
                result.status = true;
                result.message = "数据库删除成功!";
            }

            return result;
        }

        public async Task<JobResult> Update(QuarzTaskJobDao model)
        {
            var result = new JobResult { status = false, message = "" };

            var date = await _Client.Updateable(model).ExecuteCommandAsync();
            if (date > 0)
            {
                result.status = true;
                result.message = "数据库修改成功!";
            }
            return result;
        }
    }
}
