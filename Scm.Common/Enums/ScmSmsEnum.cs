namespace Com.Scm.Enums
{
    public enum ScmSmsTypeEnum
    {
        None,

        /// <summary>
        /// 收件箱（接收到的短信）
        /// </summary>
        INBOX,
        /// <summary>
        /// 已发送
        /// </summary>
        SENT,
        /// <summary>
        /// 草稿
        /// </summary>
        DRAFT,
        /// <summary>
        /// 发件箱（正在发送）
        /// </summary>
        OUTBOX,
        /// <summary>
        /// 发送失败
        /// </summary>
        FAILED,
        /// <summary>
        /// 待发送（排队中）
        /// </summary>
        QUEUED
    }

    /// <summary>
    /// 短信协议
    /// </summary>
    public enum ScmSmsProtocolEnum
    {
        None,
        Sms,
        Mms
    }

    public enum ScmSmsReadEnum
    {
        /// <summary>
        /// 未读
        /// </summary>
        Unread,
        /// <summary>
        /// 已读
        /// </summary>
        Readed
    }

    public enum ScmSmsStatusEnum
    {
        /// <summary>
        /// 接收
        /// </summary>
        Received = -1,
        /// <summary>
        /// 0 完整
        /// </summary>
        Ready = 0,
        DD = 1,
        /// <summary>
        /// 待发送 
        /// </summary>
        NeedSend = 2,
        /// <summary>
        /// 队列
        /// </summary>
        Queuing = 3,
        /// <summary>
        /// 发送中 6 待发送
        /// </summary>
        Sending = 5,
    }
}
