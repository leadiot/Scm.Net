using Com.Scm.Quartz.Dao;

namespace Com.Scm.Quartz
{
    /// <summary>
    /// 任务管理服务
    /// </summary>
    public interface IQuartzService
    {
        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        Task<List<QuarzTaskJobDao>> GetJobs();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<JobResult> AddJob(QuarzTaskJobDao taskOptions);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<JobResult> Update(QuarzTaskJobDao taskOptions);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<JobResult> Remove(QuarzTaskJobDao taskOptions);

        /// <summary>
        /// 初始化任务
        /// </summary>
        Task InitJobs();

        /// <summary>
        /// 是否为定时任务
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<JobResult> IsQuartzJob(string taskName, string groupName);

        /// <summary>
        /// 是否为表达式
        /// </summary>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        JobResult IsValidExpression(string cronExpression);

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<JobResult> Pause(QuarzTaskJobDao taskOptions);

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<JobResult> Run(QuarzTaskJobDao taskOptions);

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<JobResult> Start(QuarzTaskJobDao taskOptions);
    }
}