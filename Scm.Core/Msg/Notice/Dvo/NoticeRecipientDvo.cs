using Com.Scm.Dvo;

namespace Com.Scm.Msg.Notice.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class NoticeRecipientDvo : ScmDataDvo
    {
        /// <summary>
        /// 通知编号
        /// </summary>
        public long notice_id { get; set; }

        /// <summary>
        /// 读取次数
        /// </summary>
        public int read_qty { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 展示姓名
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }
    }
}
