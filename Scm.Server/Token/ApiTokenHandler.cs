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
/// Api 方案认证处理器 — 适用于 Web 前端。
/// 从 Authorization: Api &lt;jwt&gt; 读取令牌并验证签名和有效期。
/// 与 Bearer（标准 JWT）和 App（设备令牌）三者通过 scheme 前缀区分。
/// </summary>
public class ApiTokenHandler : AuthHandlerBase
{
    public const string SchemeName = "ApiToken";

    protected override string ExpectedScheme => ScmToken.SCHEME_API;

    public ApiTokenHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> ValidateTokenAsync(string credentials)
    {
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
        var principal = handler.ValidateToken(credentials, validationParameters, out _);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
