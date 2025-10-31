using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmFileTypeEnum
    {
        None = 0,

        [Description("字节文件")]
        Byte = 10,

        [Description("执行程序")]
        Exe = 11,
        [Description("类库文件")]
        Dll = 12,

        [Description("字符文件")]
        Text = 20,
        [Description("日志文件")]
        Logs = 21,
        [Description("代码文件")]
        Code = 22,

        [Description("媒体文件")]
        Media = 30,
        [Description("图像文件")]
        Image = 31,
        [Description("音频文件")]
        Audio = 32,
        [Description("视频文件")]
        Vedio = 33,

        [Description("办公文件")]
        Office = 40,

        [Description("归档文件")]
        Archive = 50,
    }
}
