using System.ComponentModel;

namespace Com.Scm.Enums
{
    /// <summary>
    /// 数据状态
    /// </summary>
    public enum ScmRowStatusEnum
    {
        Reverse = -1,
        /// <summary>
        /// 
        /// </summary>
        [Description("默认")]
        Normal = 0,
        /// <summary>
        /// 
        /// </summary>
        [Description("启用")]
        Enabled = 1,
        /// <summary>
        /// 
        /// </summary>
        [Description("禁用")]
        Disabled = 2
    }
}
