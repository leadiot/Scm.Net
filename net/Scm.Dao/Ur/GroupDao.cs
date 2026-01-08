using Com.Scm.Dao;
using Com.Scm.Utils;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 群组表
    /// </summary>
    [SqlSugar.SugarTable("scm_ur_group")]
    public class GroupDao : ScmDataDao, IResDao
    {
        /// <summary>
        /// 
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string names { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string namec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            codes = UidUtils.NextCodes("scm_ur_group");
            if (string.IsNullOrEmpty(names))
            {
                names = namec;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);
            if (string.IsNullOrEmpty(names))
            {
                names = namec;
            }
        }
    }
}
