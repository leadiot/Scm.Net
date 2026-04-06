using Com.Scm.Dvo;
using Com.Scm.Samples.Book.Enums;

namespace Com.Scm.Samples.Book.Dvo
{
    public class BookDvo : ScmDataDvo
    {
        /// <summary>
        /// 书籍类型
        /// </summary>
        public BookTypesEnum types { get; set; }
        /// <summary>
        /// 系统编码
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 书籍编码
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string names { get; set; }
        /// <summary>
        /// 书籍名称
        /// </summary>
        public string namec { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        public string barcode { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
