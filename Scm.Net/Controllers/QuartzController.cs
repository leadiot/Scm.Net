using Com.Scm.Controllers;
using Com.Scm.Quartz;
using Com.Scm.Quartz.Dao;
using Com.Scm.Quartz.Enums;
using Com.Scm.Quartz.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers
{
    [ApiExplorerSettings(GroupName = "Scm")]
    public class QuartzController : ApiController
    {
        private readonly IQuartzService _jobService;
        private readonly IQuartzLogService _logService;

        public QuartzController(IQuartzService jobService, IQuartzLogService logService)
        {
            _jobService = jobService;
            _logService = logService;
        }

        /// <summary>
        /// 执行任务Http
        /// </summary>
        /// <returns></returns>
        [HttpGet("job"), AllowAnonymous]
        public IActionResult TestJob()
        {
            LogUtils.Info("执行任务：" + DateTime.Now);
            return Ok("Success");
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var jobs = await _jobService.GetJobs();
            return Ok(jobs);
        }

        /// <summary>
        /// 新建任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuarzTaskJobDao model)
        {
            var data = await _jobService.AddJob(model);
            model.handle = JobHandleEnum.Paused;
            return Ok(data);
        }

        /// <summary>
        /// 开启任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("start")]
        public async Task<IActionResult> PutStartJob([FromBody] QuarzTaskJobDao model)
        {
            var data = await _jobService.Start(model);
            return Ok(data);
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("pause")]
        public async Task<IActionResult> PutPauseJob([FromBody] QuarzTaskJobDao model)
        {
            var data = await _jobService.Pause(model);
            return Ok(data);
        }

        /// <summary>
        /// 立即执行任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("run")]
        public async Task<IActionResult> PutRunJob([FromBody] QuarzTaskJobDao model)
        {
            var data = await _jobService.Run(model);
            return Ok(data);
        }

        /// <summary>
        /// 修改任务
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] QuarzTaskJobDao model)
        {
            var data = await _jobService.Update(model);
            return Ok(data);
        }

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <returns></returns>
        [HttpPut("delete")]
        public async Task<IActionResult> PutDelete([FromBody] QuarzTaskJobDao model)
        {
            var date = await _jobService.Remove(model);
            return Ok(date);
        }

        /// <summary>
        /// 获取任务执行记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("log")]
        public async Task<IActionResult> GetLogAsync(string taskName, string groupName, int current, int size)
        {
            var data = await _logService.GetLogs(taskName, groupName, current, size);
            return Ok(data);
        }
    }
}