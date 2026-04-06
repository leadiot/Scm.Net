using System.ComponentModel;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 监控状态
    /// </summary>
    public enum NasWatchEnums
    {
        None,
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        Running,
        /// <summary>
        /// 暂停中
        /// </summary>
        [Description("暂停中")]
        Suspend,
        /// <summary>
        /// 已中止
        /// </summary>
        [Description("已中止")]
        Stoped
    }
}
