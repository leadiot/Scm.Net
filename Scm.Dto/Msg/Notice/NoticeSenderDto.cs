using Com.Scm.Dto;
using Com.Scm.Enums;

namespace Com.Scm.Msg.Notice
{
    /// <summary>
    /// 发件相关信息
    /// </summary>
    public class NoticeSenderDto : ScmDto
    {
        /// <summary>
        /// 消息状态
        /// </summary>
        public ScmHandleEnum handle { get; set; }

        /// <summary>
        /// 是否草稿
        /// </summary>
        public bool is_draft { get { return handle == ScmHandleEnum.Todo; } }
    }
}
