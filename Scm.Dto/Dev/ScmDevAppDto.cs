using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmDevAppDto : ScmDto
    {
        /// <summary>
        /// 系统默认ID
        /// </summary>
        public const long SYS_ID = 1000000000000000001L;
        /// <summary>
        /// Scm.Net ID
        /// </summary>
        public const long NET_ID = 1000000000000001001L;

        /// <summary>
        /// 应用类型
        /// </summary>
        [Required]
        public int types { get; set; }

        /// <summary>
        /// 应用代码
        /// </summary>
        [StringLength(16)]
        public string code { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [StringLength(32)]
        public string name { get; set; }

        /// <summary>
        /// 应用简介
        /// </summary>
        [StringLength(1024)]
        public string content { get; set; }
    }
}
