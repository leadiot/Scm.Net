using Com.Scm.Dvo;

namespace Com.Scm.Log.Sms.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class LogOtpDvo : ScmDataDvo
    {
        /// <summary>
        /// 身份标识
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public long sms_id { get; set; }

        /// <summary>
        /// 模板代码
        /// </summary>
        public string sms_codec { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public int types { get; set; } = 0;

        /// <summary>
        /// 终端号码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 校验码
        /// </summary>
        public string sms { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 发送次数
        /// </summary>
        public int send_qty { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public long send_time { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long expired { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public int handle { get; set; }
    }
}