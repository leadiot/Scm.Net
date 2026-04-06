using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Table
{
    /// <summary>
    /// 
    /// </summary>
    public class SysTableHeaderDto : ScmDataDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        [StringLength(32)]
        public string codes { get; set; }

        /// <summary>
        /// 视图编码
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        [StringLength(32)]
        public string names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SysTableDetailDto> details { get; set; }
    }
}