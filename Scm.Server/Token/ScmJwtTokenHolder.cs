using Com.Scm.Utils;

namespace Com.Scm.Token;

/// <summary>
/// 上下文
/// </summary>
public class ScmJwtTokenHolder : IJwtTokenHolder
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
        LogUtils.Debug("SetToken: " + token.ToJsonString());
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
    /// 清除上下文
    /// </summary>
    public void Clear()
    {
        // 正确做法：清除值但不释放对象
        if (_threadLocalTenant.IsValueCreated)
        {
            _threadLocalTenant.Value = null;
        }
    }
}