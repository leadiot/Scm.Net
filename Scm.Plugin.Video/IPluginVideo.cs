namespace Com.Scm.Plugin.Vedio
{
    public interface IPluginVideo : IPlugin
    {
        bool IsWritableFile(string ext);

        bool IsReadableFile(string ext);
    }
}
