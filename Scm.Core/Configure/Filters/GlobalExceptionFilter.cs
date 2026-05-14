using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Response;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Com.Scm.Configure.Filters
{
    /// <summary>
    /// 全局异常处理过滤器
    /// </summary>
    public class GlobalExceptionHandlerFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<GlobalExceptionHandlerFilter> _logger;

        public GlobalExceptionHandlerFilter(
            IWebHostEnvironment hostEnvironment,
            ILogger<GlobalExceptionHandlerFilter> logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return;
            }

            var exception = context.Exception;
            var request = context.HttpContext.Request;

            // 记录异常日志
            LogException(context, exception, request);

            // 构建统一响应
            var result = BuildErrorResponse(exception, request);

            context.Result = new JsonResult(result)
            {
                StatusCode = GetHttpStatusCode(exception)
            };

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        private void LogException(ExceptionContext context, Exception exception, HttpRequest request)
        {
            var controllerName = (context.ActionDescriptor as ControllerActionDescriptor)?.ControllerName ?? "Unknown";
            var actionName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName ?? "Unknown";

            _logger.LogError(exception,
                "[API异常] {Controller}/{Action} | {Method} {Path} | {Message}",
                controllerName,
                actionName,
                request.Method,
                request.Path,
                exception.Message);
        }

        /// <summary>
        /// 构建错误响应
        /// </summary>
        private ApiResult BuildErrorResponse(Exception exception, HttpRequest request)
        {
            var result = new ApiResult
            {
                Path = request.Path,
                Success = false
            };

            switch (exception)
            {
                case ValidationException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    break;

                case NotFoundException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    break;

                case UnauthorizedException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    break;

                case ForbiddenException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    break;

                case BusinessException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    break;

                case ApiException ex:
                    result.Code = ex.Code;
                    result.Message = ex.Message;
                    break;

                default:
                    // 生产环境隐藏详细错误信息
                    if (_hostEnvironment.IsProduction())
                    {
                        result.Code = (int)ResultCodeEnum.InternalServerError;
                        result.Message = "服务器内部错误，请稍后重试";
                    }
                    else
                    {
                        result.Code = (int)ResultCodeEnum.InternalServerError;
                        result.Message = $"[{exception.GetType().Name}] {exception.Message}";
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// 获取 HTTP 状态码
        /// </summary>
        private int GetHttpStatusCode(Exception exception)
        {
            return exception switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                NotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                ForbiddenException => (int)HttpStatusCode.Forbidden,
                BusinessException => (int)HttpStatusCode.OK,
                ApiException => (int)HttpStatusCode.OK,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }

    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        readonly IWebHostEnvironment _hostEnvironment;
        private ILogService _logService;
        private ScmContextHolder _jwtHolder;

        public GlobalExceptionFilter(IWebHostEnvironment hostEnvironment
            , ILogService logService
            , ScmContextHolder jwtHolder)
        {
            _hostEnvironment = hostEnvironment;
            _logService = logService;
            _jwtHolder = jwtHolder;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return;
            }

            var token = _jwtHolder.GetToken();

            #region 保存异常日志
            var type = (context.ActionDescriptor as ControllerActionDescriptor)?.ControllerTypeInfo.AsType();
            if (type != null)
            {
                var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
                var now = DateTime.Now;
                var logInfo = new LogInfo()
                {
                    level = ScmLogLevelEnum.Error,
                    types = ScmLogTypesEnum.Operate,
                    module = type.FullName,
                    method = context.HttpContext.Request.Method,
                    operate_user = token.user_name,
                    ip = context.HttpContext.Connection.RemoteIpAddress?.ToString(),
                    url = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString,
                    browser = ServerUtils.GetBrowser(userAgent),
                    agent = userAgent,
                    message = context.Exception.Message,
                    operate_date = TimeUtils.FormatDate(now),
                    operate_time = TimeUtils.GetUnixTime(now),
                };
                //保存日志信息
                await _logService.LogAsync(logInfo);
            }
            #endregion

            var result = new ScmApiResponse();
            if (context.Exception is BusinessException e)
            {
                result.Code = e.Code;
                result.Message = e.Message;
            }
            else
            {
                result.Code = (int)HttpStatusCode.InternalServerError;
                result.Message = context.Exception.Message;
            }

            context.Result = new JsonResult(result);
            context.ExceptionHandled = true;
        }
    }

    /// <summary>
    /// 异步全局异常过滤器
    /// </summary>
    public class AsyncGlobalExceptionHandlerFilter : IAsyncExceptionFilter
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<AsyncGlobalExceptionHandlerFilter> _logger;

        public AsyncGlobalExceptionHandlerFilter(
            IWebHostEnvironment hostEnvironment,
            ILogger<AsyncGlobalExceptionHandlerFilter> logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return;
            }

            var exception = context.Exception;
            var request = context.HttpContext.Request;

            // 记录异常日志
            var controllerName = (context.ActionDescriptor as ControllerActionDescriptor)?.ControllerName ?? "Unknown";
            var actionName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName ?? "Unknown";

            _logger.LogError(exception,
                "[API异步异常] {Controller}/{Action} | {Method} {Path} | {Message}",
                controllerName,
                actionName,
                request.Method,
                request.Path,
                exception.Message);

            // 构建统一响应
            var result = new ApiResult
            {
                Path = request.Path,
                Success = false,
                Code = (int)ResultCodeEnum.InternalServerError,
                Message = _hostEnvironment.IsProduction()
                    ? "服务器内部错误，请稍后重试"
                    : $"[{exception.GetType().Name}] {exception.Message}"
            };

            context.Result = new JsonResult(result)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            context.ExceptionHandled = true;

            await Task.CompletedTask;
        }
    }
}