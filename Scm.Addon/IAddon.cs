namespace Com.Scm.Addon
{
    /// <summary>
    /// 扩展
    /// </summary>
    public interface IAddon
    {
        AddonType Type { get; }

        string Name { get; }
    }
}
