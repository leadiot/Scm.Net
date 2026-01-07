using Com.Scm.Dao;
using SqlSugar;

namespace Com.Scm.Sys.Pv
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_sys_pv_detail")]
    public class PvDetailDao : ScmDao
    {
        /// <summary>
        /// 日期：格式(yyyy-MM-dd)
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public long user_id { get; set; }
        /// <summary>
        /// 页面
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public long time { get; set; }
    }
}
