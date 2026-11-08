using Com.Scm.Quartz.Dao;
using Com.Scm.Quartz.Enums;
using Com.Scm.Quartz.Jobs;
using Com.Scm.Quartz.Service;
using Com.Scm.Utils;
using Quartz;

namespace Com.Scm.Quartz
{
    public class QuartzService : IQuartzService
    {
        private ISchedulerFactory _schedulerFactory;
        private IQuartzJobService _quartzJobService;
        private IQuartzLogService _quartzLogService;

        public QuartzService(ISchedulerFactory schedulerFactory,
            IQuartzJobService quartzJobService,
            IQuartzLogService quartzLogService)
        {
            _quartzJobService = quartzJobService;
            _schedulerFactory = schedulerFactory;
            _quartzLogService = quartzLogService;
        }

        /// <summary>
        /// 获取所有的作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <returns></returns>
        public async Task<List<QuarzTaskJobDao>> GetJobs()
        {
            List<QuarzTaskJobDao> list = new List<QuarzTaskJobDao>();
            try
            {
                list = await _quartzJobService.GetJobs();
                var scheduler = await _schedulerFactory.GetScheduler();
                var jobs = await scheduler.QueryJobs(new JobQuery { Take = PagedQuery.All });
                foreach (var job in jobs.Items)
                {
                    QuarzTaskJobDao taskOption = list.Where(x => x.group == job.Key.Group && x.names == job.Key.Name)
                        .FirstOrDefault();
                    if (taskOption == null)
                    {
                        continue;
                    }

                    var triggers = await scheduler.QueryTriggers(new TriggerQuery { Job = job.Key, Take = PagedQuery.All });
                    foreach (var trigger in triggers.Items)
                    {
                        if (trigger.PreviousFireTimeUtc != null)
                        {
                            taskOption.last_time = trigger.PreviousFireTimeUtc.Value.LocalDateTime;
                        }
                        else
                        {
                            var runlog = await _quartzLogService.GetLastlog(taskOption.names, taskOption.group);
                            if (runlog != null)
                            {
                                taskOption.last_time = runlog.begin_time;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Error("获取作业异常：" + ex.Message + ex.StackTrace);
            }
            return list;
        }

        public JobResult IsValidExpression(string cronExpression)
        {
            try
            {
                var expression = CronExpression.Parse(cronExpression);
                DateTimeOffset? date = expression.GetNextValidTimeAfter(DateTimeOffset.Now);
                var iscron = date != null;
                return new JobResult { status = iscron, message = date == null ? $"请确认表达式{cronExpression}是否正确!" : "" };
            }
            catch (Exception)
            {
                return new JobResult { status = false, message = $"请确认表达式{cronExpression}是否正确!" };
            }
        }

        public async Task InitJobs()
        {
            var jobs = await _quartzJobService.GetJobs();
            IScheduler scheduler = await _schedulerFactory.GetScheduler();

            foreach (var item in jobs)
            {
                try
                {
                    IJobDetail job = null;
                    if (item.types == TaskTypeEnum.Dll)
                    {
                        job = JobBuilder.Create<DllMethodJob>()
                        .WithIdentity(item.names, item.group)
                        .Build();
                    }
                    else
                    {
                        job = JobBuilder.Create<ApiClientJob>()
                        .WithIdentity(item.names, item.group)
                        .Build();
                    }

                    ITrigger trigger = TriggerBuilder.Create()
                       .WithIdentity(item.names, item.group)
                       .WithDescription(item.remark)
                       .WithCronSchedule(item.cron)
                       .Build();

                    if (item.handle == JobHandleEnum.Running)
                    {
                        await scheduler.ScheduleJob(job, trigger);
                        await _quartzLogService.AddLog(new QuarzTaskLogDao() { task = item.names, group = item.group, begin_time = DateTime.Now, remark = $"任务初始化启动成功:{item.handle}" });
                    }
                    else
                    {
                        await scheduler.ScheduleJob(job, trigger);
                        await Pause(item);
                        LogUtils.Error($"任务初始化,未启动,状态为:{item.handle}");
                    }
                }
                catch (Exception ex)
                {
                    await _quartzLogService.AddLog(new QuarzTaskLogDao() { task = item.names, group = item.group, remark = $"任务初始化未启动,出现异常,异常信息{ex.Message}" });
                    continue;
                }
            }

            await scheduler.Start();
        }

        /// <summary>
        /// 添加作业
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <param name="schedulerFactory"></param>
        /// <returns></returns>
        public async Task<JobResult> AddJob(QuarzTaskJobDao taskOptions)
        {
            JobResult result = null;
            try
            {
                var validExpression = IsValidExpression(taskOptions.cron);
                if (!validExpression.status)
                {
                    return validExpression;
                }

                var jobList = await _quartzJobService.GetJobs(a => a.names == taskOptions.names && a.group == taskOptions.group);
                if (jobList.Count > 0)
                {
                    return new JobResult { status = false, message = "任务已存在,添加失败!" };
                }

                result = await _quartzJobService.AddJob(taskOptions);
                IJobDetail job = null;
                if (taskOptions.types == TaskTypeEnum.Dll)
                {
                    job = JobBuilder.Create<DllMethodJob>()
                        .WithIdentity(taskOptions.names, taskOptions.group)
                        .Build();
                }
                else
                {
                    job = JobBuilder.Create<ApiClientJob>()
                        .WithIdentity(taskOptions.names, taskOptions.group)
                        .Build();
                }

                //  IJobDetail job = JobBuilder.Create<HttpResultfulJob>()
                // .WithIdentity(taskOptions.TaskName, taskOptions.GroupName)
                //.Build();
                ITrigger trigger = TriggerBuilder.Create()
                   .WithIdentity(taskOptions.names, taskOptions.group)
                   .WithDescription(taskOptions.remark)
                   .WithCronSchedule(taskOptions.cron)
                   .Build();

                IScheduler scheduler = await _schedulerFactory.GetScheduler();

                //if (jobFactory == null)
                //{
                //    try
                //    {
                //        jobFactory = HttpContext.Current.RequestServices.GetService<IJobFactory>();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine($"创建任务[{taskOptions.TaskName}]异常,{ex.Message}");
                //    }
                //}

                //开启才加入Schedule中,如果加入在暂停而定时任务执行过快,会导致卡死
                if (taskOptions.handle == JobHandleEnum.Running)
                {
                    await scheduler.ScheduleJob(job, trigger);
                    await scheduler.Start();
                }
                else
                {
                    await Pause(taskOptions);
                    await _quartzLogService.AddLog(new QuarzTaskLogDao() { task = taskOptions.names, group = taskOptions.group, remark = $"任务新建,未启动,状态为:{taskOptions.handle}" });
                    //FileQuartz.WriteStartLog($"作业:{taskOptions.TaskName},分组:{taskOptions.GroupName},新建时未启动原因,状态为:{taskOptions.Status}");
                }
                //if (!init)
                //{
                //    await _quartzService.AddJob(taskOptions);
                //}
                // FileQuartz.WriteJobAction(JobAction.新增, taskOptions.TaskName, taskOptions.GroupName);

            }
            catch (Exception ex)
            {
                return new JobResult { status = false, message = ex.Message };
            }

            if (result.status)
            {
                result.message = "任务添加成功!";
            }

            return result;// new ResultQuartzData { status = , message = "任务添加成功!" };
        }

        /// <summary>
        /// 移除作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task<JobResult> Remove(QuarzTaskJobDao taskOptions)
        {
            var isjob = await IsQuartzJob(taskOptions.names, taskOptions.group);
            var taskmodle = (await _quartzJobService.GetJobs(a => a.names == taskOptions.names && a.group == taskOptions.group)).FirstOrDefault();
            string message = "";
            if (isjob.status)
            {
                try
                {
                    IScheduler scheduler = await _schedulerFactory.GetScheduler();
                    var trigger = await FindTrigger(scheduler, taskOptions.group, taskOptions.names);
                    await scheduler.PauseTrigger(trigger.Key);
                    await scheduler.UnscheduleJob(trigger.Key);// 移除触发器
                    await scheduler.DeleteJob(trigger.JobKey);
                }
                catch (Exception ex)
                {
                    message += ex.Message;
                }
            }
            if (taskmodle != null)
            {
                isjob = await _quartzJobService.Remove(taskmodle);
            }
            message += isjob.message;

            return new JobResult { status = isjob.status, message = message };
        }

        /// <summary>
        /// 更新作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public async Task<JobResult> Update(QuarzTaskJobDao taskOptions)
        {
            var isjob = await IsQuartzJob(taskOptions.names, taskOptions.group);
            var taskmodle = (await _quartzJobService.GetJobs(a => a.id == taskOptions.id)).FirstOrDefault();
            var message = "";
            if (isjob.status) //如果Quartz存在就更新
            {
                try
                {
                    IScheduler scheduler = await _schedulerFactory.GetScheduler();
                    var triggerold = await FindTrigger(scheduler, taskOptions.group, taskOptions.names);
                    await scheduler.PauseTrigger(triggerold.Key);
                    await scheduler.UnscheduleJob(triggerold.Key);// 移除触发器
                    await scheduler.DeleteJob(triggerold.JobKey);
                    IJobDetail job = null;
                    if (taskOptions.types == TaskTypeEnum.Dll)
                    {
                        job = JobBuilder.Create<DllMethodJob>()
                        .WithIdentity(taskOptions.names, taskOptions.group)
                        .Build();
                    }
                    else
                    {
                        job = JobBuilder.Create<ApiClientJob>()
                        .WithIdentity(taskOptions.names, taskOptions.group)
                        .Build();
                    }
                    //  IJobDetail job = JobBuilder.Create<HttpResultfulJob>()
                    // .WithIdentity(taskOptions.TaskName, taskOptions.GroupName)
                    //.Build();
                    ITrigger triggernew = TriggerBuilder.Create()
                       .WithIdentity(taskOptions.names, taskOptions.group)
                       .StartNow()
                       .WithDescription(taskOptions.remark)
                       .WithCronSchedule(taskOptions.cron)
                       .Build();

                    await scheduler.ScheduleJob(job, triggernew);
                    if (taskOptions.handle == JobHandleEnum.Running)
                    {
                        await scheduler.Start();
                    }
                    else
                    {
                        await scheduler.PauseTrigger(triggernew.Key);
                        await _quartzLogService.AddLog(new QuarzTaskLogDao() { task = taskOptions.names, group = taskOptions.group, remark = $"任务新建,未启动,状态为:{taskOptions.handle}" });
                    }
                    message += "quarz已更新,";
                }
                catch (Exception ex)
                {
                    message += ex.Message;
                }
            }
            if (taskmodle != null)
            {
                isjob = await _quartzJobService.Update(taskOptions);
                message += isjob.message;
            }

            return new JobResult { status = isjob.status, message = message };
        }

        /// <summary>
        /// 暂停作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public async Task<JobResult> Pause(QuarzTaskJobDao taskOptions)
        {
            try
            {
                var isjob = await IsQuartzJob(taskOptions.names, taskOptions.group);
                var taskDao = (await _quartzJobService.GetJobs(a => a.names == taskOptions.names && a.group == taskOptions.group)).FirstOrDefault();

                if (isjob.status)
                {
                    IScheduler scheduler = await _schedulerFactory.GetScheduler();
                    var trigger = await FindTrigger(scheduler, taskOptions.group, taskOptions.names);
                    await scheduler.PauseTrigger(trigger.Key);
                    isjob.message += "Quartz已暂停";
                }

                if (taskDao != null)
                {
                    taskDao.handle = JobHandleEnum.Paused;

                    var date = await _quartzJobService.Update(taskDao);
                    isjob.status = date.status;
                    isjob.message += date.message;
                }

                return isjob;
            }
            catch (Exception ex)
            {
                return new JobResult { status = false, message = ex.Message };
            }
        }

        /// <summary>
        /// 启动作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public async Task<JobResult> Start(QuarzTaskJobDao taskOptions)
        {
            try
            {
                var isjob = await IsQuartzJob(taskOptions.names, taskOptions.group);
                var taskDao = (await _quartzJobService.GetJobs(a => a.names == taskOptions.names && a.group == taskOptions.group)).FirstOrDefault();
                taskDao.handle = JobHandleEnum.Running;

                IScheduler scheduler = await _schedulerFactory.GetScheduler();
                if (!isjob.status) //如果不存在则加入
                {
                    IJobDetail job = null;
                    if (taskOptions.types == TaskTypeEnum.Dll)
                    {
                        job = JobBuilder.Create<DllMethodJob>()
                        .WithIdentity(taskOptions.names, taskOptions.group)
                        .Build();
                    }
                    else
                    {
                        job = JobBuilder.Create<ApiClientJob>()
                        .WithIdentity(taskOptions.names, taskOptions.group)
                        .Build();
                    }

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity(taskOptions.names, taskOptions.group)
                        .WithDescription(taskOptions.remark)
                        .WithCronSchedule(taskOptions.cron)
                        .Build();

                    await scheduler.ScheduleJob(job, trigger);
                    await scheduler.Start();
                }
                else //存在则直接启动
                {
                    var trigger = await FindTrigger(scheduler, taskOptions.group, taskOptions.names);
                    await scheduler.ResumeTrigger(trigger.Key);
                }

                var date = await _quartzJobService.Update(taskDao);
                return date;
            }
            catch (Exception ex)
            {
                return new JobResult { status = false, message = ex.Message };
            }
        }

        /// <summary>
        /// 立即执行一次作业
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        public async Task<JobResult> Run(QuarzTaskJobDao taskOptions)
        {
            try
            {
                var isjob = await IsQuartzJob(taskOptions.names, taskOptions.group);
                var taskmodle = (await _quartzJobService.GetJobs(a => a.names == taskOptions.names && a.group == taskOptions.group)).FirstOrDefault();
                if (isjob.status)
                {
                    //taskmodle.Status = (int)JobState.立即执行;
                    IScheduler scheduler = await _schedulerFactory.GetScheduler();
                    var trigger = await FindTrigger(scheduler, taskOptions.group, taskOptions.names);
                    await scheduler.TriggerJob(trigger.JobKey);
                    return new JobResult { status = true, message = $"{taskOptions.names}立即执行任务成功" };
                }
                else
                {
                    return isjob;
                }

            }
            catch (Exception ex)
            {
                return new JobResult { status = false, message = ex.Message };
            }
        }

        /// <summary>
        /// 判断是否存在此任务
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task<JobResult> IsQuartzJob(string taskName, string groupName)
        {
            try
            {
                string errorMsg = "";
                IScheduler scheduler = await _schedulerFactory.GetScheduler();
                var jobs = await scheduler.QueryJobs(new JobQuery
                {
                    Group = GroupMatcher<JobKey>.GroupEquals(groupName),
                    Take = 1
                });
                if (jobs.Items.Count == 0)
                {
                    errorMsg = $"未找到分组[{groupName}]";
                    return new JobResult { status = false, message = errorMsg };
                }

                var trigger = await FindTrigger(scheduler, groupName, taskName);
                if (trigger == null)
                {
                    errorMsg = $"未找到任务{taskName}]";
                    return new JobResult { status = false, message = errorMsg };
                }

                return new JobResult { status = true, message = errorMsg };
            }
            catch (Exception ex)
            {
                return new JobResult { status = false, message = ex.Message };
            }
        }

        /// <summary>
        /// 查找指定分组下名称匹配的触发器
        /// </summary>
        private async Task<TriggerHeader> FindTrigger(IScheduler scheduler, string groupName, string triggerName)
        {
            var triggers = await scheduler.QueryTriggers(new TriggerQuery
            {
                Group = GroupMatcher<TriggerKey>.GroupEquals(groupName),
                Name = NameMatcher<TriggerKey>.NameEquals(triggerName),
                Take = 1
            });
            return triggers.Items.FirstOrDefault();
        }
    }
}
