namespace Com.Scm.Log
{
    public enum SmsTypesEnum
    {
        None = 0,
        Phone = 1,
        Email = 2
    }

    public enum SmsHandleEnum
    {
        None = 0,
        /// <summary>
        /// 待发送
        /// </summary>
        Todo,
        /// <summary>
        /// 发送中
        /// </summary>
        Doing,
        /// <summary>
        /// 发送成功
        /// </summary>
        Success,
        /// <summary>
        /// 发送失败
        /// </summary>
        Failure
    }
}
