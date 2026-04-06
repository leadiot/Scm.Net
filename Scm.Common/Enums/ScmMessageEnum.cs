using System.ComponentModel;

namespace Com.Scm.Enums
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum ScmMessageTypeEnum
    {
        None = 0,
        [Description("系统消息")]
        System = 1,
        [Description("应用消息")]
        App = 2,
        [Description("用户消息")]
        User = 4,
        [Description("其它消息")]
        Order = 9,
    }

    /// <summary>
    /// 消息状态
    /// </summary>
    public enum ScmMessageHandleEnum
    {
        None = 0,
        [Description("未读")]
        UnRead = 1,
        [Description("已读")]
        Readed = 2
    }
}