using Com.Scm.Config;
using Com.Scm.Scalar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Com.Scm
{
    public static class ScalarExtension
    {
        /// <summary>
        /// 兜底文档名：收纳未分组端点与不在 ApiDocs 配置中的孤儿分组，确保所有接口在 Scalar 中可见
        /// </summary>
        private const string OTHER_DOC_NAME = "other";

        private const string OTHER_DOC_TITLE = "其他接口";

        public static void ScalarSetup(this IServiceCollection services, ScalarConfig config)
        {
            if (config == null)
            {
                return;
            }

            services.AddTransient<BearerSecuritySchemeTransformer>();

            if (config.HasDocs())
            {
                foreach (var doc in config.ApiDocs)
                {
                    services.AddOpenApi(doc.Group);
                }
                services.AddOpenApi(OTHER_DOC_NAME);

                //var configuredGroups = config.ApiDocs
                //    .Select(d => d.Group)
                //    .Where(g => !string.IsNullOrEmpty(g))
                //    .ToList();

                //foreach (var apiDoc in config.ApiDocs)
                //{
                //    var groupName = apiDoc.Group;
                //    // 文档名统一以小写注册：AddOpenApi 内部会将文档名规范化为小写注册命名选项/键控服务，
                //    // 直接使用小写可保证注册、路由解析、Scalar 下拉框三个环节名称一致
                //    services.AddOpenApi(groupName.ToLower(), options =>
                //    {
                //        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                //        // 用配置的中文标题/描述/版本回填文档 info（否则默认为 "{AppName} | {documentName}"）
                //        options.AddDocumentTransformer(new OpenApiInfoTransformer(config, apiDoc));
                //        // 只收纳对应分组的端点（忽略大小写比较，避免 GroupName 与文档名大小写不一致）
                //        options.ShouldInclude = description =>
                //            string.Equals(description.GroupName, groupName, StringComparison.OrdinalIgnoreCase);
                //        options.ApplyXmlComments(config);
                //    });
                //}

                //// 兜底文档：收纳未分组端点（GroupName == null）与配置之外的孤儿分组，
                //// 否则这部分接口不会出现在任何文档中
                //services.AddOpenApi(OTHER_DOC_NAME, options =>
                //{
                //    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                //    options.AddDocumentTransformer(new OpenApiInfoTransformer(config, new ApiInfo
                //    {
                //        Title = OTHER_DOC_TITLE,
                //        Description = "未配置分组的接口（含未分组端点与未纳入 ApiDocs 的分组）",
                //        Version = config.Version
                //    }));
                //    options.ShouldInclude = description =>
                //        description.GroupName == null ||
                //        !configuredGroups.Any(g => string.Equals(description.GroupName, g, StringComparison.OrdinalIgnoreCase));
                //    options.ApplyXmlComments(config);
                //});
            }
            else
            {
                //// 注册默认文档 "v1"
                //services.AddOpenApi("v1", options =>
                //{
                //    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                //    options.AddDocumentTransformer(new OpenApiInfoTransformer(config, null));
                //    options.ApplyXmlComments(config);
                //});

                services.AddOpenApi();
            }
        }

        public static void UseScalarSetup(this WebApplication app, ScalarConfig config)
        {
            if (config == null)
            {
                return;
            }

            var openApiRoute = string.IsNullOrWhiteSpace(config.OpenApiRoute) ? "/openapi/{documentName}.json" : config.OpenApiRoute;
            //app.MapOpenApi();
            app.MapOpenApi(openApiRoute);

            var scalarRoute = string.IsNullOrWhiteSpace(config.ScalarRoute) ? "/scalar" : config.ScalarRoute;

            app.MapScalarApiReference(scalarRoute, options =>
            {
                // 1. 设置文档标题
                options.WithTitle(config.Title ?? "Scm.Net API 文档");

                // 2. 设置 OpenAPI 路由模式（关键！必须与 MapOpenApi 的路由模板匹配）
                options.WithOpenApiRoutePattern(openApiRoute);

                // 3. 添加多文档支持
                if (config.HasDocs())
                {
                    foreach (var apiDoc in config.ApiDocs)
                    {
                        // documentName 必须与 AddOpenApi 注册的小写名称一致，首个文档为默认文档
                        options.AddDocument(apiDoc.Group, title: apiDoc.Title);
                    }
                    // 兜底文档
                    options.AddDocument(OTHER_DOC_NAME, title: OTHER_DOC_TITLE);
                }
                else
                {
                    options.AddDocument("v1", title: config.Title ?? "Scm.Net API 文档");
                }

                // 4. 配置自定义服务器地址（支持多个环境）
                if (config.Servers != null && config.Servers.Count > 0)
                {
                    foreach (var server in config.Servers)
                    {
                        options.AddServer(server.Url, server.Description);
                    }
                }
            });
        }

        /// <summary>
        /// 文档名统一小写化，与 AddOpenApi 内部的规范化行为保持一致
        /// </summary>
        private static string GetDocumentName(string groupName)
        {
            return (groupName ?? string.Empty).ToLowerInvariant();
        }

        /// <summary>
        /// 应用 XML 文档注释（读取程序集生成的 XML 文件填充接口摘要与说明）
        /// </summary>
        private static void ApplyXmlComments(this OpenApiOptions options, ScalarConfig config)
        {
            if (config?.DllXmls == null || config.DllXmls.Count == 0)
            {
                return;
            }

            var files = config.DllXmls
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Path.IsPathRooted(x) ? x : Path.Combine(AppContext.BaseDirectory, x))
                .Where(File.Exists)
                .ToList();

            if (files.Count == 0)
            {
                return;
            }

            options.AddOperationTransformer(new XmlCommentsOperationTransformer(files));
        }
    }
}
