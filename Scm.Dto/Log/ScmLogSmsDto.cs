using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Log
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmLogSmsDto : ScmDataDto
    {
        /// <summary>
        /// 身份标识
        /// </summary>
        [StringLength(32)]
        public string key { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [Required]
        public long sms_id { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        [Required]
        public int types { get; set; }

        /// <summary>
        /// 终端号码
        /// </summary>
        [StringLength(128)]
        public string code { get; set; }

        /// <summary>
        /// 校验码
        /// </summary>
        [StringLength(8)]
        public string sms { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [StringLength(1024)]
        public string content { get; set; }

        /// <summary>
        /// 发送次数
        /// </summary>
        [Required]
        public int send_qty { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [Required]
        public long send_time { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Required]
        public long expired { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public int handle { get; set; }
    }
}