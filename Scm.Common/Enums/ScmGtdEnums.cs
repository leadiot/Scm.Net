namespace Com.Scm.Sys.Enums
{
    /// <summary>
    /// 优先级
    /// </summary>
    public enum ScmGtdPriorityEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 紧急且重要
        /// </summary>
        Level1 = 1,
        /// <summary>
        /// 紧急不重要
        /// </summary>
        Level2 = 2,
        /// <summary>
        /// 不紧急但重要
        /// </summary>
        Level3 = 3,
        /// <summary>
        /// 不重要不紧急
        /// </summary>
        Level4 = 4,
    }

    /// <summary>
    /// 提醒类型
    /// </summary>
    public enum ScmGtdRemindEnum
    {
        None = 0,
        /// <summary>
        /// 不提醒
        /// </summary>
        Close = 1,
        /// <summary>
        /// 提醒
        /// </summary>
        Open = 2
    }

    /// <summary>
    /// 提示方式
    /// </summary>
    public enum ScmGtdNoticeEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        Sms = 1,
        Email = 2,
        Alert = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ScmGtdHandleEnum
    {
        None = 0,
        Todo = 1,
        Doing = 2,
        Done = 3,
    }
}
