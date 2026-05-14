using Com.Scm.Configure.Filters;
using Com.Scm.Configure.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm.Extensions
{
    /// <summary>
    /// 统一响应扩展方法
    /// </summary>
    public static class UnifiedResponseExtensions
    {
        /// <summary>
        /// 添加统一响应服务
        /// </summary>
        public static IServiceCollection AddUnifiedResponse(this IServiceCollection services)
        {
            // 配置 MVC 选项
            services.Configure<MvcOptions>(options =>
            {
                // 添加全局异常过滤器
                options.Filters.Add<GlobalExceptionHandlerFilter>();

                // 添加统一响应过滤器（可选，如果不使用中间件）
                // options.Filters.Add<UnifiedResponseFilter>();
            });

            return services;
        }

        /// <summary>
        /// 使用统一响应中间件
        /// </summary>
        public static IApplicationBuilder UseUnifiedResponse(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UnifiedResponseMiddleware>();
        }

        /// <summary>
        /// 添加 API 行为配置（包含统一响应和异常处理）
        /// </summary>
        public static IServiceCollection AddApiBehavior(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // 禁用默认的 ModelState 验证响应
                options.SuppressModelStateInvalidFilter = true;

                // 自定义无效模型响应
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(e => e.Value.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var result = new Response.ApiResult
                    {
                        Success = false,
                        Code = 400,
                        Message = $"参数验证失败：{string.Join("; ", errors)}",
                        Path = context.HttpContext.Request.Path
                    };

                    return new BadRequestObjectResult(result);
                };
            });

            return services;
        }
    }
}
