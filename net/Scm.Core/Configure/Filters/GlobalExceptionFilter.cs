using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Response;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Com.Scm.Api.Configure.Filters
{
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
            var token = _jwtHolder.GetToken();

            if (context.ExceptionHandled) return;

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
                result.Code = (int)HttpStatusCode.Found;
                result.Message = e.GetMessage();
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
}