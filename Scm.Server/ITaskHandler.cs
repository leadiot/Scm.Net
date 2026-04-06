using Com.Scm.Config;
using Com.Scm.Sys.Tasks;
using Com.Scm.Utils;
using SqlSugar;

namespace Com.Scm
{
    /// <summary>
    /// 数据导入导出处理程序
    /// </summary>
    public abstract class ITaskHandler
    {
        public abstract int Types { get; }

        public abstract string Name { get; }

        public abstract string Json { get; }

        public abstract void Execute(EnvConfig config, ISqlSugarClient client, TaskDao dao);

        public virtual long FromTime()
        {
            return 0;
        }

        public virtual long ToTime()
        {
            return long.MaxValue;
        }

        protected bool IsValidId(long id)
        {
            return id > 1000;
        }

        protected bool IsNormalId(long id)
        {
            return id > ScmEnv.DEFAULT_ID;
        }

        protected bool IsValidInt(long id)
        {
            return id > 0;
        }
    }
}
