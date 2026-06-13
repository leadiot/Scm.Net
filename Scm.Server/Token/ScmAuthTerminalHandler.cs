using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Com.Scm.Token;

/// <summary>
/// App 方案认证处理器 — 适用于 IoT / 移动端设备绑定登录。
/// 从 Authorization: App &lt;base64&gt; 读取令牌并解析终端绑定信息。
/// 与 Bearer（标准 JWT）和 Api（Web 前端 JWT）三者通过 scheme 前缀区分。
/// </summary>
public class ScmAuthTerminalHandler : ScmAuthHandler
{
    public const string SchemeName = "Terminal";

    protected override string ExpectedScheme => ScmToken.SCHEME_TERMINAL;

    public ScmAuthTerminalHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> ValidateTokenAsync(string credentials)
    {
        // 复用 ScmToken.FromAppToken 统一解析 Base64 设备令牌
        var scmToken = ScmToken.FromTerminalToken(credentials);

        var identity = new System.Security.Claims.ClaimsIdentity(SchemeName);
        var principal = new System.Security.Claims.ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
