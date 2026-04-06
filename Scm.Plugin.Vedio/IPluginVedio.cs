namespace Com.Scm.Plugin.Vedio
{
    public interface IPluginVedio : IPlugin
    {
        bool IsWritableFile(string ext);

        bool IsReadableFile(string ext);
    }
}
