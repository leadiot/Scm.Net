using System.ComponentModel;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum NasTypeEnums
    {
        None,
        /// <summary>
        /// 设备
        /// </summary>
        [Description("目录")]
        Device = 1,
        /// <summary>
        /// 目录
        /// </summary>
        [Description("目录")]
        Dir = 10,
        /// <summary>
        /// 文档
        /// </summary>
        [Description("文档")]
        Doc = 20
    }

    /// <summary>
    /// 同步方向
    /// </summary>
    public enum NasDirEnums
    {
        None,
        /// <summary>
        /// 仅上传
        /// </summary>
        [Description("仅上传")]
        Upload,
        /// <summary>
        /// 双向同步
        /// </summary>
        [Description("双向同步")]
        Sync,
        /// <summary>
        /// 仅下载
        /// </summary>
        [Description("仅下载")]
        Download
    }

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
