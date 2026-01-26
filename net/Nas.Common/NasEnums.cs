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
