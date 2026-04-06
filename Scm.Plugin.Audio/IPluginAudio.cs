namespace Com.Scm.Plugin.Audio
{
    public interface IPluginAudio : IPlugin
    {
        bool IsWritableFile(string ext);

        bool IsReadableFile(string ext);
    }
}
