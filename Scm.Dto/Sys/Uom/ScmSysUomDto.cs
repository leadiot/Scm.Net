using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 计量单位
    /// </summary>
    public class ScmSysUomDto : ScmDataDto
    {
        /// <summary>
        /// 单位类型：基础单位、复合单位
        /// </summary>
        [Required]
        public ScmUomTypesEnum types { get; set; }

        /// <summary>
        /// 单位模式：长度单位、重量单位、体积单位、时间单位、币制单位
        /// </summary>
        [Required]
        public ScmUomModesEnum modes { get; set; }

        /// <summary>
        /// 单位制式：国际、市制、英制等
        /// </summary>
        [Required]
        public ScmUomKindsEnum kinds { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        [Required]
        public int od { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        [StringLength(16)]
        public string codes { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }

        /// <summary>
        /// 显示语言
        /// </summary>
        [StringLength(8)]
        public string lang { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        [StringLength(32)]
        public string names { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [StringLength(32)]
        public string namec { get; set; }

        /// <summary>
        /// 单位符号
        /// </summary>
        [StringLength(8)]
        public string symbol { get; set; }

        /// <summary>
        /// 参照单位
        /// </summary>
        public long refer_id { get; set; }

        /// <summary>
        /// 参照数量
        /// </summary>
        public decimal refer_qty { get; set; }

        /// <summary>
        /// 基准单位
        /// </summary>
        public long basic_id { get; set; }

        /// <summary>
        /// 基准数量
        /// </summary>
        public decimal basic_qty { get; set; }
    }
}