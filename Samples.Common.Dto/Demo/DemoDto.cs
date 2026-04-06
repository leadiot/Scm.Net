using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Samples.Demo
{
    /// <summary>
    /// 演示对象DTO
    /// </summary>
    public class DemoDto : ScmDataDto
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        [StringLength(16)]
        public string codes { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        [StringLength(32)]
        public string names { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        [StringLength(128)]
        public string namec { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(32)]
        public string phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        public string remark { get; set; }
    }
}
