using MiniExcelLibs.Attributes;

namespace Com.Scm.Samples.Book.Dvo
{
    public class BookExcelDvo
    {
        /// <summary>
        /// 书籍编码
        /// </summary>
        [ExcelColumnName("书籍编码")]
        public string codec { get; set; }
        /// <summary>
        /// 书籍名称
        /// </summary>
        [ExcelColumnName("书籍名称")]
        public string namec { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [ExcelColumnName("条码")]
        public string barcode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [ExcelColumnName("备注")]
        public string remark { get; set; }
    }
}
