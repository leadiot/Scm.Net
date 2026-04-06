using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Service;
using Com.Scm.Sys.Tasks;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Reflection;

namespace Com.Scm.Tasks.DataIO
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Scm")]
    public class DataIOService : ApiService
    {
        private static bool _Running;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="envConfig"></param>
        /// <param name="sqlClient"></param>
        public DataIOService(EnvConfig envConfig, ISqlSugarClient sqlClient)
        {
            _EnvConfig = envConfig;
            _SqlClient = sqlClient;
        }

        /// <summary>
        /// 定时任务调用接口
        /// </summary>
        [AllowAnonymous]
        public async Task<bool> GetRunAsync()
        {
            if (_Running)
            {
                return _Running;
            }

            var now = TimeUtils.GetUnixTime();
            var needQty = await _SqlClient.Updateable<TaskDao>()
                .SetColumns(a => a.handle == ScmHandleEnum.Todo)
                .SetColumns(a => a.exec_time_f == now)
                .Where(a => a.handle == ScmHandleEnum.Todo && a.row_status == Enums.ScmRowStatusEnum.Enabled && now >= a.need_time_f && now < a.need_time_t)
                .ExecuteCommandAsync();
            if (needQty == 0)
            {
                return false;
            }

            _Running = true;
            new Thread(() =>
            {
                DoRun(now);
            }).Start();

            return true;
        }

        private void DoRun(long time)
        {
            var tasks = _SqlClient.GetList<TaskDao>(a => a.handle == ScmHandleEnum.Todo && a.exec_time_f == time);
            var realQty = 0;
            foreach (var task in tasks)
            {
                var handler = GetInstance(task.clazz);
                if (handler == null)
                {
                    task.result = ScmResultEnum.Failure;
                    task.message = "无效的导出任务：" + task.clazz;
                    _SqlClient.Updateable(task).ExecuteCommand();
                    continue;
                }

                realQty += 1;
                // 标记执行状态
                task.handle = ScmHandleEnum.Doing;
                task.exec_time_f = time;
                _SqlClient.Updateable(task).ExecuteCommand();

                // 执行
                handler.Execute(_EnvConfig, _SqlClient, task);
            }
            _Running = false;
        }

        private static ITaskHandler GetInstance(string key)
        {
            try
            {
                var obj = Assembly.GetExecutingAssembly().CreateInstance(key);
                if (obj != null)
                {
                    if (obj is ITaskHandler)
                    {
                        return (ITaskHandler)obj;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex);
            }

            return null;
        }
    }
}
