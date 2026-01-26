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
        Delete,
        /// <summary>
        /// 创建
        /// </summary>
        [Description("创建")]
        Create,
        /// <summary>
        /// 更名
        /// </summary>
        [Description("更名")]
        Rename,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Change,
        /// <summary>
        /// 移动
        /// </summary>
        [Description("移动")]
        Move,
        /// <summary>
        /// 复制
        /// </summary>
        [Description("复制")]
        Copy,
        /// <summary>
        /// 压缩
        /// </summary>
        [Description("压缩")]
        Compress,
        /// <summary>
        /// 解压
        /// </summary>
        [Description("解压")]
        Decompress,
        /// <summary>
        /// 分享
        /// </summary>
        [Description("分享")]
        Share,
        /// <summary>
        /// 解除分享
        /// </summary>
        [Description("解除分享")]
        Unshare,
        /// <summary>
        /// 隐私
        /// </summary>
        [Description("隐私")]
        Lock,
        /// <summary>
        /// 解除隐私
        /// </summary>
        [Description("解除隐私")]
        Unlock,
        /// <summary>
        /// 恢复
        /// </summary>
        [Description("恢复")]
        Restore,
    }
}
