using Com.Scm.Config;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Com.Scm
{
    public static class JwtExtension
    {
        public static void SetupJwt(this IServiceCollection services, EnvConfig envConfig)
        {
            services.AddScoped(typeof(ScmContextHolder));

            var section = AppUtils.GetConfig(JwtConfig.Name);
            services.Configure<JwtConfig>(section);
            var jwtConfig = section.Get<JwtConfig>();
            jwtConfig.Prepare(envConfig);

            services.AddAuthentication(x =>
            {
                // 指定默认认证方案（JwtBearer 作为主方案）
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // 认证失败时的默认挑战方案
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                // 验证令牌的有效性参数
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // 验证签发者
                    ValidateIssuer = true,
                    // 签发者（与令牌签发时一致）
                    ValidIssuer = jwtConfig.Issuer,
                    // 验证受众
                    ValidateAudience = true,
                    // 受众（与令牌签发时一致）
                    ValidAudience = jwtConfig.Audience,
                    // 验证签名密钥
                    ValidateIssuerSigningKey = true,
                    // 签名密钥（与令牌签发时一致，必须保密）
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Security)),
                    // 验证令牌过期时间
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                };
                x.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        var values = context.Request.Headers[ScmToken.ApiToken];
                        var token = values.FirstOrDefault();

                        if (!string.IsNullOrEmpty(token))
                        {
                            // 剥离 "Api " 前缀（如果存在）
                            if (token.StartsWith(ScmToken.PRE_API))
                            {
                                token = token.Substring(ScmToken.PRE_API.Length);
                            }
                        }

                        context.Token = token;
                        return Task.CompletedTask;
                    },
                };
            })
            // 注册 AppToken 认证方案（用于设备/应用绑定登录，与 JwtBearer 并行）
            .AddScheme<AuthenticationSchemeOptions, AppTokenHandler>(AppTokenHandler.SchemeName, null);

            services.AddAuthorization(options =>
            {
                // 默认策略同时接受 JwtBearer 和 AppToken 两种认证方案
                // RequireAuthorization() 全局生效时，会依次尝试两个方案
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme, AppTokenHandler.SchemeName)
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("App", policy => policy.RequireRole("App").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            });
        }
    }
}
