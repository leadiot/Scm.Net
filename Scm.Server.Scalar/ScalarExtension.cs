using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Scm.Server.Scalar.Config;

namespace Scm.Server.Scalar
{
    public static class ScalarExtension
    {
        public static void ScalarSetup(this IServiceCollection services, ScalarConfig config)
        {
            services.AddTransient<BearerSecuritySchemeTransformer>();

            services.AddOpenApi("v1", options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });
        }

        public static void UseScalarSetup(this WebApplication app, ScalarConfig config)
        {
            app.MapOpenApi(config?.OpenApiRoute ?? "/openapi/v1.json");

            app.MapScalarApiReference(options =>
            {
                // 1. 自定义文档标题
                options.Title = config?.Title ?? "Scm.Net API";

                // 2. 设置主题样式（例如：紫色主题）
                //options.Theme = ScalarTheme.Purple;

                // 3. 配置自定义服务器地址（支持多个环境）
                //options.Servers = new List<ScalarServer>
                //{
                //    new ScalarServer("http://localhost:5000", "开发环境"),
                //    new ScalarServer("http://106.14.146.154:9999", "测试环境"),
                //    new ScalarServer("https://api.c-scm.net", "生产环境"),
                //};

                // OAuth2
                options.AddAuthorizationCodeFlow("OAuth2", flow =>
                {
                    flow.WithAuthorizationUrl("/oauth/callback")
                        .WithClientId("bookings-frontend-client")
                        .WithSelectedScopes("openid", "profile", "orders")
                        .WithPkce(Pkce.Sha256); // 强制使用 PKCE 以保障前端安全
                });
            });
        }
    }
    internal sealed class BearerSecuritySchemeTransformer(
    IAuthenticationSchemeProvider authenticationSchemeProvider
) : IOpenApiDocumentTransformer
    {
        public async Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            var authenticationSchemes = await authenticationSchemeProvider.GetAllSchemesAsync();

            if (authenticationSchemes.Any(authScheme => authScheme.Name == "Bearer"))
            {
                document.Components ??= new OpenApiComponents();
                document.AddComponent("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "请输入 JWT Token"
                });
                //document.AddComponent("Operator", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "Operator",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "请输入 Operator Token"
                //});
                //document.AddComponent("Terminal", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.Http,
                //    Scheme = "Operator",
                //    BearerFormat = "JWT",
                //    In = ParameterLocation.Header,
                //    Description = "请输入 Terminal Token"
                //});

                var securityRequirements = new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                };

                // 将安全要求应用到文档中的所有操作
                foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
                {
                    operation.Value.Security ??= new List<OpenApiSecurityRequirement>();
                    operation.Value.Security.Add(securityRequirements);
                }
            }
        }
    }
}
