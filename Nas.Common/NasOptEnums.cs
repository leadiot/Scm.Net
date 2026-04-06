using System.ComponentModel;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum NasOptEnums
    {
        None,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 1,
        /// <summary>
        /// 恢复
        /// </summary>
        [Description("恢复")]
        Restore = 2,

        /// <summary>
        /// 创建
        /// </summary>
        [Description("创建")]
        Create = 11,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Change = 12,
        /// <summary>
        /// 更名
        /// </summary>
        [Description("更名")]
        Rename = 13,
        /// <summary>
        /// 移动
        /// </summary>
        [Description("移动")]
        Move = 14,
        /// <summary>
        /// 复制
        /// </summary>
        [Description("复制")]
        Copy = 15,

        /// <summary>
        /// 压缩
        /// </summary>
        [Description("压缩")]
        Compress = 21,
        /// <summary>
        /// 解压
        /// </summary>
        [Description("解压")]
        Decompress = 22,
        /// <summary>
        /// 分享
        /// </summary>
        [Description("分享")]
        Share = 23,
        /// <summary>
        /// 解除分享
        /// </summary>
        [Description("解除分享")]
        Unshare = 24,
        /// <summary>
        /// 隐私
        /// </summary>
        [Description("隐私")]
        Lock = 25,
        /// <summary>
        /// 解除隐私
        /// </summary>
        [Description("解除隐私")]
        Unlock = 26,
    }
}
