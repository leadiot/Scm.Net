using Com.Scm.Config;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;

namespace Com.Scm.Token;

/// <summary>
/// 自定义 API 令牌认证处理器（适用于 Web 前端 ApiToken 请求）
/// 从 ApiToken 请求头读取 JWT，剥离 "Api " 前缀后验证签名和有效期。
/// 与标准 JwtBearer（Authorization: Bearer）和 AppTokenHandler 三者并行工作。
/// </summary>
public class ApiTokenHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "ApiToken";

    public ApiTokenHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // 仅处理携带 ApiToken 请求头的请求
        if (!Request.Headers.ContainsKey(ScmToken.ApiToken))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        try
        {
            var tokenValue = Request.Headers[ScmToken.ApiToken].FirstOrDefault();
            if (string.IsNullOrEmpty(tokenValue))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            // 剥离 "Api " 前缀（如果存在）
            if (tokenValue.StartsWith(ScmToken.PRE_API))
            {
                tokenValue = tokenValue.Substring(ScmToken.PRE_API.Length);
            }

            // 读取 JWT 配置并构建验证参数（与 JwtBearer 使用相同的密钥和策略）
            var jwtConfig = AppUtils.GetConfig<JwtConfig>(JwtConfig.Name);
            if (jwtConfig == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("JWT 配置缺失"));
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Security)),
                ValidateLifetime = true,
                RequireExpirationTime = true,
            };

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(tokenValue, validationParameters, out _);
            var ticket = new AuthenticationTicket(principal, SchemeName);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (SecurityTokenException ex)
        {
            return Task.FromResult(AuthenticateResult.Fail($"ApiToken 验证失败: {ex.Message}"));
        }
        catch (Exception ex)
        {
            return Task.FromResult(AuthenticateResult.Fail($"ApiToken 解析异常: {ex.Message}"));
        }
    }
}
