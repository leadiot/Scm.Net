using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

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
        /// 时间
        /// </summary>
        public long time { get; set; }
    }
}
