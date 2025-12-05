using Com.Scm.Quartz.Dao;
using Com.Scm.Quartz.Enums;
using System.Linq.Expressions;

namespace Com.Scm.Quartz.Service.Df
{
    /// <summary>
    /// 基于文件的任务管理服务
    /// </summary>
    public class DfQuartzJobService : IQuartzJobService
    {
        private QuartzFileHelper _Helper;

        public DfQuartzJobService(QuartzFileHelper helper)
        {
            _Helper = helper;
        }

        public Task<JobResult> AddJob(QuarzTaskJobDao model)
        {
            return Task.Run(() =>
             {
                 var list = _Helper.GetJobs();
                 if (list == null)
                 {
                     list = new List<QuarzTaskJobDao>();
                 }

                 model.PrepareCreate(0);

                 list.Add(model);
                 _Helper.WriteJobConfig(list);
                 return new JobResult { message = "数据保存成功!", status = true };
             });
        }

        public Task<List<QuarzTaskJobDao>> GetJobs(Expression<Func<QuarzTaskJobDao, bool>> where = null)
        {
            return Task.Run(() =>
            {
                return _Helper.GetJobs(where);
            });
        }

        public Task<JobResult> Pause(QuarzTaskJobDao model)
        {
            //throw new NotImplementedException();
            return Task.Run(() =>
            {
                var list = _Helper.GetJobs();
                list.ForEach(item =>
                {
                    if (item.names == model.names && item.group == model.group)
                    {
                        item.handle = JobHandleEnum.Paused;
                    }
                });
                _Helper.WriteJobConfig(list);
                return new JobResult { message = "数据暂停成功!", status = true };
            });
        }

        public Task<JobResult> Remove(QuarzTaskJobDao model)
        {
            return Task.Run(() =>
            {
                var list = _Helper.GetJobs();
                list.Remove(list.Find(a => a.names == model.names && a.group == model.group));
                _Helper.WriteJobConfig(list);
                return new JobResult { message = "数据暂停成功!", status = true };
            });
        }

        //public Task<ResultQuartzData> Run(tab_quarz_task model)
        //{
        //    return Task.Run(() =>
        //    {
        //        var list = _quartzFileHelper.GetJobs(a => 1 == 1);
        //        list.ForEach(f => {
        //            if (f.TaskName == model.TaskName && f.GroupName == model.GroupName)
        //            {
        //                f.Status = Convert.ToInt32(JobState.开启);
        //            }
        //        });
        //        _quartzFileHelper.WriteJobConfig(list);
        //        return new ResultQuartzData { message = "数据暂停成功!", status = true };

        //    });
        //}

        public Task<JobResult> Start(QuarzTaskJobDao model)
        {
            return Task.Run(() =>
            {
                var list = _Helper.GetJobs();
                list.ForEach(item =>
                {
                    if (item.names == model.names && item.group == model.group)
                    {
                        item.handle = JobHandleEnum.Running;
                    }
                });
                _Helper.WriteJobConfig(list);
                return new JobResult { message = "数据开启成功!", status = true };
            });
        }

        public Task<JobResult> Update(QuarzTaskJobDao model)
        {
            return Task.Run(() =>
            {
                model.PrepareUpdate(0);

                var list = _Helper.GetJobs();
                list.Remove(list.Find(a => a.id == model.id));
                list.Add(model);
                _Helper.WriteJobConfig(list);
                return new JobResult { message = "数据修改成功!", status = true };
            });
        }
    }
}
