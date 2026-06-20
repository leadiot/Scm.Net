namespace Com.Scm.Enums
{
    public enum ScmCallTypeEnum
    {
        /// <summary>
        /// 已接来电
        /// </summary>
        INCOMING,
        /// <summary>
        /// 拨出电话
        /// </summary>
        OUTGOING,
        /// <summary>
        /// 未接来电
        /// </summary>
        MISSED,
        /// <summary>
        /// 语音邮件
        /// </summary>
        VOICEMAIL,
        /// <summary>
        /// 拒绝的来电
        /// </summary>
        REJECTED,
        /// <summary>
        /// 拦截的来电
        /// </summary>
        BLOCKED,
        /// <summary>
        /// 外部设备接听
        /// </summary>
        ANSWERED_EXTERNALLY
    }
}
