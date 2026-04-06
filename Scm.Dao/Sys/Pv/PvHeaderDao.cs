using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [StringLength(10)]
        [SugarColumn(Length = 10)]
        public string date { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public long user_id { get; set; }
        /// <summary>
        /// 页面
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string url { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }
    }
}
