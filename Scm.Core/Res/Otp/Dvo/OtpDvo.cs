using Com.Scm.Dvo;
using Com.Scm.Log;

namespace Com.Scm.Res.Otp.Dvo
{
    /// <summary>
    /// 消息模板
    /// </summary>
    public class OtpDvo : ScmDataDvo
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        public OtpTypesEnum types { get; set; }

        /// <summary>
        /// 模板代码
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 标题模板
        /// </summary>
        public string head { get; set; }

        /// <summary>
        /// 内容模板
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 声明模板
        /// </summary>
        public string foot { get; set; }

        /// <summary>
        /// 文件模板
        /// </summary>
        public string file { get; set; }
    }
}