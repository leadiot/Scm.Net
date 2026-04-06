namespace Com.Scm.Enums
{
    /// <summary>
    /// 审批状态
    /// </summary>
    public enum ScmWfaStatusEnum
    {
        None = 0,
        /// <summary>
        /// 待审批
        /// </summary>
        Todo = 1,
        /// <summary>
        /// 审批中
        /// </summary>
        Doing = 2,
        /// <summary>
        /// 审批中断
        /// </summary>
        Suspend = 3,
        /// <summary>
        /// 审批拒绝
        /// </summary>
        Reject = 4,
        /// <summary>
        /// 审批通过
        /// </summary>
        Accept = 5,
    }
}
