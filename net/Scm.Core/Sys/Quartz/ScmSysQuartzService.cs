using Com.Scm.Quartz;
using Com.Scm.Quartz.Dao;
using Com.Scm.Quartz.Enums;
using Com.Scm.Quartz.Service;
using Com.Scm.Sys.Quartz.Dvo;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Quartz;

/// <summary>
/// 任务调度
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysQuartzService : IApiService
{
    private readonly IQuartzService _quartzHandle;
    private readonly IQuartzLogService _logService;

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="quartzHandle"></param>
    /// <param name="logService"></param>
    public ScmSysQuartzService(IQuartzService quartzHandle, IQuartzLogService logService)
    {
        _quartzHandle = quartzHandle;
        _logService = logService;
    }

    /// <summary>
    /// 获取任务列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<QuarzTaskJobDao>> GetPagesAsync(SearchRequest request)
    {
        return await _quartzHandle.GetJobs();
    }

    /// <summary>
    /// 新建任务
    /// </summary>
    /// <returns></returns>
    public async Task<JobResult> AddAsync([FromBody] QuarzTaskJobDao model)
    {
        var date = await _quartzHandle.AddJob(model);
        model.handle = JobHandleEnum.Paused;
        return date;
    }

    /// <summary>
    /// 暂停任务
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<JobResult> PutPauseJob([FromBody] QuarzTaskJobDao model) =>
        await _quartzHandle.Pause(model);

    /// <summary>
    /// 开启任务
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<JobResult> PutStartJob([FromBody] QuarzTaskJobDao model) =>
        await _quartzHandle.Start(model);

    /// <summary>
    /// 立即执行任务
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<JobResult> PutRunJob([FromBody] QuarzTaskJobDao model) =>
        await _quartzHandle.Run(model);

    /// <summary>
    /// 修改任务
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<JobResult> Put([FromBody] QuarzTaskJobDao model) =>
        await _quartzHandle.Update(model);

    /// <summary>
    /// 删除任务
    /// </summary>
    /// <returns></returns>
    [HttpDelete]
    public async Task<JobResult> Delete([FromBody] QuarzTaskJobDao model) =>
        await _quartzHandle.Remove(model);

    /// <summary>
    /// 获取任务执行记录
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ResultData<QuarzTaskLogDao>> JobRecord(string taskName, string groupName, int current, int size) =>
        await _logService.GetLogs(taskName, groupName, current, size);
}