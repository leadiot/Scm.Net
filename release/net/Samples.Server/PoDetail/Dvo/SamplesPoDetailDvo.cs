using Com.Scm.Dvo;

namespace Com.Scm.Samples.PoDetail.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SamplesPoDetailDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 书籍ID
        /// </summary>
        public long book_id { get; set; }

        public string book_codes { get; set; }

        public string book_names { get; set; }

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
        public string remark { get; set; }
    }
}