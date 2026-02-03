using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmFileTypeEnum
    {
        None = 0,
        /// <summary>
        /// 目录
        /// </summary>
        [Description("目录")]
        Dir = 10,
        /// <summary>
        /// 文件
        /// </summary>
        [Description("文件")]
        Doc = 20
    }

    public enum ScmFileKindEnum
    {
        None = 0,

        /// <summary>
        /// 目录
        /// </summary>
        [Description("目录")]
        Dir = 10,
        /// <summary>
        /// 分类
        /// </summary>
        [Description("分类")]
        Folder = 11,
        /// <summary>
        /// 设备
        /// </summary>
        [Description("设备")]
        Device = 12,

        [Description("字节文件")]
        Byte = 20,

        [Description("执行程序")]
        Exe = 21,
        [Description("类库文件")]
        Dll = 22,

        [Description("字符文件")]
        Text = 30,
        [Description("日志文件")]
        Logs = 31,
        [Description("代码文件")]
        Code = 32,

        [Description("媒体文件")]
        Media = 40,
        [Description("图像文件")]
        Image = 41,
        [Description("音频文件")]
        Audio = 42,
        [Description("视频文件")]
        Vedio = 43,

        [Description("办公文件")]
        Office = 50,

        [Description("归档文件")]
        Archive = 60,
    }
}
