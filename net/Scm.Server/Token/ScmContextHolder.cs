namespace Com.Scm.Token;

/// <summary>
/// 上下文
/// </summary>
public class ScmContextHolder : IScmHolder
{
    /// <summary>
    /// 支持父子线程数据传递
    /// </summary>
    private readonly ThreadLocal<ScmToken> _threadLocalTenant = new();

    /// <summary>
    /// 设置租户ID
    /// </summary>
    /// <param name="token"></param>
    public void SetToken(ScmToken token)
    {
        _threadLocalTenant.Value = token;
    }

    /// <summary>
    /// 获取租户ID
    /// </summary>
    /// <returns></returns>
    public ScmToken GetToken()
    {
        try
        {
            return _threadLocalTenant.Value ?? new ScmToken();
        }
        catch
        {
            return new ScmToken();
        }
    }

    /// <summary>
    /// 清除
    /// </summary>
    public void Clear()
    {
        _threadLocalTenant.Dispose();
    }
}