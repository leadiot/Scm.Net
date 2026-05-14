using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Response;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Com.Scm.Configure.Middleware
{
    /// <summary>
    /// 统一响应格式中间件
    /// </summary>
    public class UnifiedResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UnifiedResponseMiddleware> _logger;

        public UnifiedResponseMiddleware(RequestDelegate next, ILogger<UnifiedResponseMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // 跳过不需要包装的路径
            if (IsSkipPath(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var originalBodyStream = context.Response.Body;

            try
            {
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    try
                    {
                        await _next(context);
                    }
                    catch (Exception ex)
                    {
                        await HandleExceptionAsync(context, ex);
                        return;
                    }

                    // 如果响应已经被处理（如文件下载），直接返回
                    if (context.Response.ContentType != null && 
                        (context.Response.ContentType.Contains("application/octet-stream") ||
                         context.Response.ContentType.Contains("multipart/form-data")))
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                        return;
                    }

                    // 读取响应内容
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseContent = await new StreamReader(responseBody).ReadToEndAsync();

                    // 如果已经是统一格式或空响应，直接返回
                    if (string.IsNullOrEmpty(responseContent) || IsAlreadyWrapped(responseContent))
                    {
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                        return;
                    }

                    // 包装响应
                    await WrapResponseAsync(context, responseContent, originalBodyStream);
                }
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "请求处理发生异常");

            var result = new ApiResult();
            result.Path = context.Request.Path;

            switch (exception)
            {
                case ValidationException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case NotFoundException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case UnauthorizedException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case ForbiddenException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;

                case BusinessException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    break;

                case ApiException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    break;

                default:
                    result.Code = (int)ResultCodeEnum.InternalServerError;
                    result.Message = "服务器内部错误，请稍后重试";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await context.Response.WriteAsync(json);
        }

        /// <summary>
        /// 包装响应
        /// </summary>
        private async Task WrapResponseAsync(HttpContext context, string responseContent, Stream originalBodyStream)
        {
            ApiResult<object> wrappedResult;

            // 根据状态码判断成功或失败
            if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
            {
                // 尝试解析为 JSON
                object data = null;
                try
                {
                    data = JsonSerializer.Deserialize<object>(responseContent);
                }
                catch
                {
                    data = responseContent;
                }

                wrappedResult = new ApiResult<object>
                {
                    Success = true,
                    Code = context.Response.StatusCode,
                    Message = "操作成功",
                    Data = data,
                    Path = context.Request.Path
                };
            }
            else
            {
                wrappedResult = new ApiResult<object>
                {
                    Success = false,
                    Code = context.Response.StatusCode,
                    Message = responseContent,
                    Path = context.Request.Path
                };
            }

            context.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(wrappedResult, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await originalBodyStream.WriteAsync(System.Text.Encoding.UTF8.GetBytes(json));
        }

        /// <summary>
        /// 检查是否已经是统一格式
        /// </summary>
        private bool IsAlreadyWrapped(string content)
        {
            try
            {
                using (var doc = JsonDocument.Parse(content))
                {
                    var root = doc.RootElement;
                    return root.TryGetProperty("success", out _) &&
                           root.TryGetProperty("code", out _) &&
                           root.TryGetProperty("message", out _);
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查是否跳过包装
        /// </summary>
        private bool IsSkipPath(PathString path)
        {
            var pathValue = path.Value?.ToLower() ?? "";
            return pathValue.Contains("/swagger") ||
                   pathValue.Contains("/health") ||
                   pathValue.Contains("/hangfire") ||
                   pathValue.Contains("/quartz") ||
                   pathValue.Contains("/signalr") ||
                   pathValue.Contains("/ws");
        }
    }
}
