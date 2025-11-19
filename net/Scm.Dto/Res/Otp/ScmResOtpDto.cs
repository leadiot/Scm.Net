using Com.Scm.Dto;
using Com.Scm.Log;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Otp
{
    /// <summary>
    /// 消息模板
    /// </summary>
    public class ScmResOtpDto : ScmDataDto
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        public OtpTypesEnum types { get; set; }

        /// <summary>
        /// 模板代码
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        [StringLength(64)]
        public string namec { get; set; }

        /// <summary>
        /// 标题模板
        /// </summary>
        [StringLength(128)]
        public string head { get; set; }

        /// <summary>
        /// 内容模板
        /// </summary>
        [StringLength(512)]
        public string body { get; set; }

        /// <summary>
        /// 声明模板
        /// </summary>
        [StringLength(128)]
        public string foot { get; set; }

        /// <summary>
        /// 文件模板
        /// </summary>
        [StringLength(64)]
        public string file { get; set; }


    }
}