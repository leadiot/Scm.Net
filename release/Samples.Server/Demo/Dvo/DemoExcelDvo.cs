using MiniExcelLibs.Attributes;

namespace Com.Scm.Samples.Demo.Dvo
{
    public class DemoExcelDvo
    {
        /// <summary>
        /// 客户编码
        /// </summary>
        [ExcelColumnName("客户编码")]
        public string codec { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        [ExcelColumnName("客户名称")]
        public string namec { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [ExcelColumnName("电话")]
        public string phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [ExcelColumnName("备注")]
        public string remark { get; set; }
    }
}
