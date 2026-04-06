namespace Com.Scm.Plugin
{
    /// <summary>
    /// 插件
    /// </summary>
    public interface IPlugin
    {
        PluginType Type { get; }

        string Name { get; }
    }
}
