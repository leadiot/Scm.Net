namespace Com.Scm.Token;

/// <summary>
/// 上下文
/// </summary>
public class ScmJwtTokenHolder : IJwtTokenHolder
{
    /// <summary>
    /// 支持异步上下文数据传递（跨 await 边界）
    /// </summary>
    private readonly AsyncLocal<ScmToken> _asyncLocal = new();

    /// <summary>
    /// 设置租户ID
    /// </summary>
    /// <param name="token"></param>
    public void SetToken(ScmToken token)
    {
        _asyncLocal.Value = token;
    }

    /// <summary>
    /// 获取租户ID
    /// </summary>
    /// <returns></returns>
    public ScmToken GetToken()
    {
        try
        {
            return _asyncLocal.Value ?? new ScmToken();
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
        // AsyncLocal 不需要 Dispose，直接清除值即可
        _asyncLocal.Value = null;
    }
}