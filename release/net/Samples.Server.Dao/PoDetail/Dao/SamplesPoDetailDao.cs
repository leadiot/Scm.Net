using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Samples.PoDetail.Dao
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("samples_po_detail")]
    public class SamplesPoDetailDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public long book_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 需求数量
        /// </summary>
        public int need_qty { get; set; }

        /// <summary>
        /// 实际数量
        /// </summary>
        public int real_qty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(128)]
        public string remark { get; set; }
    }
}