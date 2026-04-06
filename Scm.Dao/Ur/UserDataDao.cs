using Com.Scm.Dao;
using Com.Scm.Enums;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 用户数据关系表
    /// </summary>
    [SqlSugar.SugarTable("scm_ur_user_data")]
    public class UserDataDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmUserDataTypesEnum types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long data_id { get; set; }
    }
}
