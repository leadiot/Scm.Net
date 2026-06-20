namespace Com.Scm.Enums
{
    public enum ScmSmsTypeEnum
    {
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
}
