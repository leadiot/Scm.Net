using Com.Scm.Utils;
using SqlSugar;

namespace Com.Scm.Dao
{
    public class ScmDao
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long id { get; set; }

        public virtual void PrepareCreate(long userId)
        {
            if (!IsValidId())
            {
                id = UidUtils.NextId();
            }
        }

        public virtual void PrepareUpdate(long userId)
        {
        }

        public virtual void PrepareDelete(long userId)
        {
        }

        /// <summary>
        /// 批量数据删除
        /// </summary>
        /// <returns></returns>
        public virtual List<string> DmlBatchDelete()
        {
            return null;
        }

        /// <summary>
        /// 批量新增数量
        /// </summary>
        /// <returns></returns>
        public virtual List<ScmDao> DmlBatchInsert(long userId, long time)
        {
            return null;
        }

        public bool IsValidId()
        {
            return id > 1000;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var dto = obj as ScmDao;
            return dto != null && dto.id == id;
        }
    }
}
