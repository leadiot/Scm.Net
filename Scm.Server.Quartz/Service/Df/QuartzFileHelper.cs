using Com.Scm.Quartz.Config;
using Com.Scm.Quartz.Dao;
using Com.Scm.Utils;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;

namespace Com.Scm.Quartz.Service.Df
{
    public class QuartzFileHelper
    {
        private QuartzConfig _Config;
        private readonly object _lockObj = new();

        public QuartzFileHelper(QuartzConfig config)
        {
            _Config = config;
        }

        /// <summary>
        /// 获取jobs
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<QuarzTaskJobDao> GetJobs(Expression<Func<QuarzTaskJobDao, bool>> where = null)
        {
            List<QuarzTaskJobDao> list = new List<QuarzTaskJobDao>();

            string path = _Config.JobFile;
            if (!File.Exists(path))
            {
                return list;
            }

            string tasks;
            lock (_lockObj)
            {
                tasks = FileUtils.ReadText(path);
            }

            if (string.IsNullOrEmpty(tasks))
            {
                return null;
            }

            var _taskList = tasks.AsJsonObject<List<QuarzTaskJobDao>>();
            if (where == null)
            {
                return _taskList;
            }

            return _taskList.Where(where.Compile()).ToList();
        }

        /// <summary>
        /// 读取任务日志
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<QuarzTaskLogDao> GetJobRunLog(string taskName, string groupName, int page, int pageSize = 100)
        {
            List<QuarzTaskLogDao> list = new List<QuarzTaskLogDao>();

            string path = Path.Combine(_Config.LogsDir, groupName, taskName);
            if (!File.Exists(path))
            {
                return list;
            }

            var logs = ReadPageLine(path, page, pageSize, true);
            foreach (string item in logs)
            {
                string[] arr = item?.Split('_');
                if (item == "" || arr == null || arr.Length == 0)
                    continue;
                if (arr.Length != 3)
                {
                    list.Add(new QuarzTaskLogDao() { remark = item });
                    continue;
                }
                list.Add(new QuarzTaskLogDao() { begin_time = Convert.ToDateTime(arr[0]), end_time = Convert.ToDateTime(arr[1]), remark = arr[2] });
            }

            return list.OrderByDescending(x => x.begin_time).ToList();
        }

        /// <summary>
        /// 写入任务(全量)
        /// </summary>
        /// <param name="taskList"></param>
        public void WriteJobConfig(List<QuarzTaskJobDao> taskList)
        {
            string jobs = taskList.ToJsonString();
            lock (_lockObj)
            {
                FileUtils.WriteText(_Config.JobFile, jobs);
            }
        }

        public void WriteStartLog(string content)
        {
            content = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + content + Environment.NewLine;
            lock (_lockObj)
            {
                FileUtils.WriteText(_Config.GetLogsFile("start.txt"), content, true);
            }
        }

        public void WriteJobLogs(QuarzTaskLogDao log)
        {
            var content = log.ToJsonString() + Environment.NewLine;
            lock (_lockObj)
            {
                FileUtils.WriteText(_Config.GetLogsFile("logs.txt"), content, true);
            }
        }

        public List<QuarzTaskLogDao> GetJobsLog(int pageSize = 1)
        {
            string path = _Config.GetLogsFile("logs.txt");
            if (!File.Exists(path))
                return null;

            var listlogs = ReadPageLine(path, pageSize, 5000, true).ToList();
            List<QuarzTaskLogDao> listtasklogs = new List<QuarzTaskLogDao>();
            foreach (var item in listlogs)
            {
                listtasklogs.Add(JsonSerializer.Deserialize<QuarzTaskLogDao>(item));
            }
            return listtasklogs;
        }

        /// <summary>
        /// 读取本地txt日志内容
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="seekEnd"></param>
        /// <returns></returns>
        public IEnumerable<string> ReadPageLine(string fullPath, int page, int pageSize, bool seekEnd = false)
        {
            if (page <= 0)
            {
                page = 1;
            }
            var lines = File.ReadLines(fullPath, Encoding.UTF8);
            if (seekEnd)
            {
                int lineCount = lines.Count();
                int linPageCount = (int)Math.Ceiling(lineCount / (pageSize * 1.00));
                //超过总页数，不处理
                if (page > linPageCount)
                {
                    page = 0;
                    pageSize = 0;
                }
                else if (page == linPageCount)//最后一页，取最后一页剩下所有的行
                {
                    pageSize = lineCount - (page - 1) * pageSize;
                    if (page == 1)
                    {
                        page = 0;
                    }
                    else
                    {
                        page = lines.Count() - page * pageSize;
                    }
                }
                else
                {
                    page = lines.Count() - page * pageSize;
                }
            }
            else
            {
                page = (page - 1) * pageSize;
            }
            lines = lines.Skip(page).Take(pageSize);

            var enumerator = lines.GetEnumerator();
            int count = 1;
            while (enumerator.MoveNext() || count <= pageSize)
            {
                yield return enumerator.Current;
                count++;
            }
            enumerator.Dispose();
        }
    }
}
