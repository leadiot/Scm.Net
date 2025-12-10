using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Samples.PoDetail.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class SamplesPoDetailDto : ScmDataDto
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