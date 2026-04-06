using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Ur.User.Dvo;

namespace Com.Scm.Msg.Chat.Group.Dvo
{
    /// <summary>
    /// 群组
    /// </summary>
    public class ChatGroupDvo : ScmDataDvo
    {
        /// <summary>
        /// 群组类型（个人，群聊）
        /// </summary>
        public ScmChatGroupTypesEnum types { get; set; }

        /// <summary>
        /// 群组模式（外部群、内部群）
        /// </summary>
        public ScmChatGroupModesEnums modes { get; set; }

        /// <summary>
        /// 群组名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 人员限制
        /// </summary>
        public int max { get; set; }

        /// <summary>
        /// 人员数量
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 群组摘要
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SimpleUserDvo> users { get; set; }
    }
}