using Com.Scm.Quartz.Dao;
using System.Linq.Expressions;

namespace Com.Scm.Quartz.Service
{
    /// <summary>
    /// 任务服务
    /// </summary>
    public interface IQuartzJobService
    {
        /// <summary>
        /// 获取所有作业
        /// </summary>
        /// <returns></returns>
        Task<List<QuarzTaskJobDao>> GetJobs(Expression<Func<QuarzTaskJobDao, bool>> where = null);

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<JobResult> AddJob(QuarzTaskJobDao model);

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<JobResult> Remove(QuarzTaskJobDao model);

        /// <summary>
        /// 更新任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        Task<JobResult> Update(QuarzTaskJobDao model);
    }
}
