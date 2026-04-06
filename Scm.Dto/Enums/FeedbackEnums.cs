using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum FeedbackTypesEnums
    {
        None = 0,
    }

    public enum FeedbackHandleEnums
    {
        None = 0,
        [Description("待回复")]
        Todo = 1,
        [Description("处理中")]
        Doing = 2,
        [Description("已完成")]
        Done = 3,
    }
}
