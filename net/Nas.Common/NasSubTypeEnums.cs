using System.ComponentModel;

namespace Com.Scm.Nas
{
    public enum NasSubTypeEnums
    {
        None = 0,

        /// <summary>
        /// 分类
        /// </summary>
        [Description("分类")]
        DirFolder = 11,

        /// <summary>
        /// 设备
        /// </summary>
        [Description("设备")]
        DirDevice = 12,

        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        DocImage = 21,
        /// <summary>
        /// 音频
        /// </summary>
        [Description("音频")]
        DocAudio = 22,

        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        DocVideo = 23,

        /// <summary>
        /// 媒体
        /// </summary>
        [Description("媒体")]
        DocMedia = 24,

        /// <summary>
        /// 办公
        /// </summary>
        [Description("办公")]
        DocOffice = 25,

        /// <summary>
        /// 代码
        /// </summary>
        [Description("代码")]
        DocScript = 26,

        /// <summary>
        /// 归档
        /// </summary>
        [Description("归档")]
        DocArchive = 27,
    }
}
