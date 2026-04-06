using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Chat
{
    /// <summary>
    /// 群组
    /// </summary>
    public class ChatGroupDto : ScmDataDto
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
        [StringLength(64)]
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
        [StringLength(256)]
        public string hash { get; set; }
    }
}