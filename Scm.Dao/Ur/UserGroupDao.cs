using Com.Scm.Dao;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 用户群组关系表
    /// </summary>
    [SqlSugar.SugarTable("scm_ur_user_group")]
    public class UserGroupDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long group_id { get; set; }
    }
}
