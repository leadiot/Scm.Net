using Com.Scm.Dao;
using SqlSugar;

namespace Com.Scm.Sys.Pv
{
    /// <summary>
    /// 页面访问统计
    /// </summary>
    [SugarTable("scm_sys_pv_header")]
    public class PvHeaderDao : ScmDao
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
        /// 数量
        /// </summary>
        public int qty { get; set; }
    }
}
