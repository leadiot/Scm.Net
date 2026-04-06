namespace Com.Scm.Enums
{
    /// <summary>
    /// 阅读状态
    /// </summary>
    public enum NoticeReadedEnum
    {
        None = 0,
        /// <summary>
        /// 未读
        /// </summary>
        Unread = 1,
        /// <summary>
        /// 已读
        /// </summary>
        Readed = 2
    }

    /// <summary>
    /// 发送状态
    /// </summary>
    public enum NoticeSendEnum
    {
        None,
        Sending,
        Done,
        Fail
    }

    public enum NoticeTypesEnum
    {
        None = 0,
        /// <summary>
        /// 正常邮件
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 需要回复
        /// </summary>
        NeedReply = 2,
        /// <summary>
        /// 不可回复
        /// </summary>
        NoReply = 3
    }
}