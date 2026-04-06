namespace Com.Scm.Quartz
{
    /// <summary>
    /// 定制服务
    /// </summary>
    public interface ICustomJob
    {
        string ExecuteService(string parameter);
    }
}
