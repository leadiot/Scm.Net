using Com.Scm.Config;
using Com.Scm.Scalar;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Com.Scm
{
    public static class ScalarExtension
    {
        public static void ScalarSetup(this IServiceCollection services, ScalarConfig config)
        {
            services.AddTransient<BearerSecuritySchemeTransformer>();

            // 注册配置中的多文档
            if (config.HasDocs())
            {
                foreach (var apiDoc in config.ApiDocs)
                {
                    services.AddOpenApi(apiDoc.Group, options =>
                    {
                        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    });
                }
            }
            else
            {
                // 注册默认文档 "v1"
                services.AddOpenApi("v1", options =>
                {
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                });
            }
        }

        public static void UseScalarSetup(this WebApplication app, ScalarConfig config)
        {
            if (config == null)
            {
                return;
            }

            // MapOpenApi 使用路由模板，{documentName} 占位符会匹配 AddOpenApi 注册的文档名称
            app.MapOpenApi("/openapi/{documentName}.json");

            // 使用配置中的 Scalar 路由
            var scalarRoute = config?.ScalarRoute ?? "/scalar";

            app.MapScalarApiReference(scalarRoute, options =>
            {
                // 1. 设置文档标题
                options.WithTitle(config?.Title ?? "Scm.Net API 文档");

                // 2. 设置 OpenAPI 路由模式（关键！必须与 MapOpenApi 的路由模板匹配）
                options.WithOpenApiRoutePattern("/openapi/{documentName}.json");

                // 3. 添加多文档支持
                if (config.HasDocs())
                {
                    // 使用 AddDocument 方法逐个添加文档（推荐方式）
                    // 参数：documentName（必须与 AddOpenApi 注册的名称一致）、title、routePattern
                    foreach (var apiDoc in config.ApiDocs)
                    {
                        // 添加文档，并设置标题（routePattern 使用默认值即可）
                        options.AddDocument(apiDoc.Group, title: apiDoc.Title);
                    }
                }
                else
                {
                    // 默认只添加 v1 文档
                    options.AddDocument("v1", title: config?.Title ?? "Scm.Net API 文档");
                }

                // 4. 配置自定义服务器地址（支持多个环境）
                if (config?.Servers != null && config.Servers.Count > 0)
                {
                    foreach (var server in config.Servers)
                    {
                        options.AddServer(server.Url, server.Description);
                    }
                }

                // 5. OAuth2 认证配置
                options.AddAuthorizationCodeFlow("OAuth2", flow =>
                {
                    flow.WithAuthorizationUrl("/oauth/callback")
                        .WithClientId("bookings-frontend-client")
                        .WithSelectedScopes("openid", "profile", "orders")
                        .WithPkce(Pkce.Sha256);
                });
            });
        }
    }
}
