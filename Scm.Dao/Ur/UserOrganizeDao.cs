using Com.Scm.Dao;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 用户机构关系表
    /// </summary>
    [SqlSugar.SugarTable("scm_ur_user_organize")]
    public class UserOrganizeDao : ScmDataDao
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }
        /// <summary>
        /// 组织ID
        /// </summary>
        public long organize_id { get; set; }

        ///// <summary>
        ///// 是否负责人
        ///// </summary>
        //public bool owner { get; set; }
    }
}
