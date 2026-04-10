using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Gtd
{
    /// <summary>
    /// 待办（备注）
    /// </summary>
    [SugarTable("scm_gtd_comment")]
    public class GtdCommentDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        public string comment { get; set; }
    }
}
