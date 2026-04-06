using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Msg.Message.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageDvo : ScmDataDvo
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ScmMessageTypeEnum types { get; set; }

        /// <summary>
        /// 留言标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 邮箱信息
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 留言内容
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool isread { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool is_del { get; set; }

        /// <summary>
        /// 留言标签
        /// </summary>
        public List<TagDvo> tags { get; set; }
    }
}
