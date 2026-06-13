using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Com.Scm.Token;

/// <summary>
/// 认证处理器基类 — 统一从 Authorization 请求头按 scheme 前缀分发。
/// 子类只需指定期望的 scheme 名称并实现 ValidateToken 方法。
/// 遵循 RFC 7235：Authorization: &lt;scheme&gt; &lt;credentials&gt;
/// </summary>
public abstract class AuthHandlerBase : AuthenticationHandler<AuthenticationSchemeOptions>
{
    /// <summary>
    /// 此处理器期望的 Authorization scheme 前缀（如 "Api"、"App"）
    /// </summary>
    protected abstract string ExpectedScheme { get; }

    protected AuthHandlerBase(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authHeader = Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        // 提取 credentials：scheme 不匹配则交给下一个处理器
        var credentials = ScmToken.GetCredentials(authHeader, ExpectedScheme);
        if (credentials == null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        try
        {
            return ValidateTokenAsync(credentials);
        }
        catch (Exception ex)
        {
            return Task.FromResult(AuthenticateResult.Fail($"{ExpectedScheme} 认证失败: {ex.Message}"));
        }
    }

    /// <summary>
    /// 子类实现具体的令牌验证逻辑
    /// </summary>
    /// <param name="credentials">剥离 scheme 前缀后的原始凭据</param>
    /// <returns>认证成功返回 Success，否则返回 Fail</returns>
    protected abstract Task<AuthenticateResult> ValidateTokenAsync(string credentials);
}
