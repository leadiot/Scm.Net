using Com.Scm.Dao;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 用户岗位关系表
    /// </summary>
    [SqlSugar.SugarTable("scm_ur_user_position")]
    public class UserPositionDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long position_id { get; set; }
    }
}
