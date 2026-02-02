using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Chat
{
    /// <summary>
    /// 群组
    /// </summary>
    [SugarTable("scm_msg_chat_group")]
    public class ChatGroupDao : ScmDataDao
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
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
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
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string hash { get; set; }
    }
}