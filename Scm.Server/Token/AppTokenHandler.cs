using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Com.Scm.Token;

/// <summary>
/// 设备令牌认证处理器（适用于 IoT/移动端 AppToken 请求）
/// 当请求携带 AppToken 请求头时，解析设备绑定令牌并完成认证。
/// 与 JwtBearer 方案并行工作，由 JwtMiddleware 统一调度上下文。
/// </summary>
public class AppTokenHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "AppToken";

    public AppTokenHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // 仅处理携带 AppToken 请求头的请求
        if (!Request.Headers.ContainsKey(ScmToken.AppToken))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        try
        {
            var tokenValue = Request.Headers[ScmToken.AppToken].FirstOrDefault();
            if (string.IsNullOrEmpty(tokenValue))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            // 复用 ScmToken.FromAppToken 统一解析
            var scmToken = ScmToken.FromAppToken(tokenValue);

            var identity = new System.Security.Claims.ClaimsIdentity(SchemeName);
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception ex)
        {
            return Task.FromResult(AuthenticateResult.Fail($"AppToken 解析失败: {ex.Message}"));
        }
    }
}
