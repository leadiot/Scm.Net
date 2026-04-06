using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmSysUomDvo : ScmDataDvo
    {
        /// <summary>
        /// 单位类型：基础单位、复合单位
        /// </summary>
        public ScmUomTypesEnum types { get; set; }

        /// <summary>
        /// 单位模式：长度单位、重量单位、体积单位、时间单位、币制单位
        /// </summary>
        public ScmUomModesEnum modes { get; set; }

        /// <summary>
        /// 单位制式：国际、市制、英制等
        /// </summary>
        public ScmUomKindsEnum kinds { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        public string codes { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 显示语言
        /// </summary>
        public string lang { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 单位符号
        /// </summary>
        public string symbol { get; set; }

        /// <summary>
        /// 参数单位
        /// </summary>
        public long refer_id { get; set; }

        public string refer_names { get; set; }

        /// <summary>
        /// 参数数量
        /// </summary>
        public decimal refer_qty { get; set; }

        /// <summary>
        /// 基准单位
        /// </summary>
        public long basic_id { get; set; }

        public string basic_names { get; set; }

        /// <summary>
        /// 基准数量
        /// </summary>
        public decimal basic_qty { get; set; }
    }
}