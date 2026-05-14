using Com.Scm.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Com.Scm.Configure.Filters
{
    /// <summary>
    /// 统一 API 响应格式过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class UnifiedResponseFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            // 如果已经有异常，让异常过滤器处理
            if (context.Exception != null)
            {
                return;
            }

            // 如果结果已经是统一格式，不再包装
            if (context.Result is JsonResult jsonResult && jsonResult.Value is ApiResult)
            {
                return;
            }

            // 处理各种结果类型
            switch (context.Result)
            {
                case ObjectResult objectResult:
                    context.Result = WrapObjectResult(objectResult, context);
                    break;

                case JsonResult result:
                    context.Result = WrapJsonResult(result, context);
                    break;

                case OkResult:
                    context.Result = new JsonResult(ApiResult.Ok())
                    {
                        StatusCode = 200
                    };
                    break;

                case NotFoundResult:
                    context.Result = new JsonResult(ApiResult.Error("资源不存在", 404))
                    {
                        StatusCode = 404
                    };
                    break;

                case BadRequestResult:
                    context.Result = new JsonResult(ApiResult.Error("请求参数错误", 400))
                    {
                        StatusCode = 400
                    };
                    break;

                case ContentResult contentResult:
                    // 如果是字符串内容，包装为数据响应
                    context.Result = new JsonResult(ApiResult<object>.Ok(contentResult.Content))
                    {
                        StatusCode = 200
                    };
                    break;

                case EmptyResult:
                case NoContentResult:
                    context.Result = new JsonResult(ApiResult.Ok())
                    {
                        StatusCode = 200
                    };
                    break;

                case FileResult:
                    // 文件下载不包装
                    break;

                case StatusCodeResult statusCodeResult:
                    if (statusCodeResult.StatusCode >= 200 && statusCodeResult.StatusCode < 300)
                    {
                        context.Result = new JsonResult(ApiResult.Ok())
                        {
                            StatusCode = statusCodeResult.StatusCode
                        };
                    }
                    else
                    {
                        context.Result = new JsonResult(ApiResult.Error($"请求失败，状态码：{statusCodeResult.StatusCode}", statusCodeResult.StatusCode))
                        {
                            StatusCode = statusCodeResult.StatusCode
                        };
                    }
                    break;
            }

            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 包装 ObjectResult
        /// </summary>
        private IActionResult WrapObjectResult(ObjectResult result, ActionExecutedContext context)
        {
            // 如果已经是 ApiResult 类型，不再包装
            if (result.Value is ApiResult)
            {
                return result;
            }

            var apiResult = new ApiResult<object>
            {
                Success = result.StatusCode >= 200 && result.StatusCode < 300,
                Code = result.StatusCode ?? 200,
                Message = result.StatusCode >= 200 && result.StatusCode < 300 ? "操作成功" : "操作失败",
                Data = result.Value,
                Path = context.HttpContext.Request.Path
            };

            return new JsonResult(apiResult)
            {
                StatusCode = result.StatusCode ?? 200
            };
        }

        /// <summary>
        /// 包装 JsonResult
        /// </summary>
        private IActionResult WrapJsonResult(JsonResult result, ActionExecutedContext context)
        {
            // 如果已经是 ApiResult 类型，不再包装
            if (result.Value is ApiResult)
            {
                return result;
            }

            var apiResult = new ApiResult<object>
            {
                Success = true,
                Code = result.StatusCode ?? 200,
                Message = "操作成功",
                Data = result.Value,
                Path = context.HttpContext.Request.Path
            };

            return new JsonResult(apiResult)
            {
                StatusCode = result.StatusCode ?? 200
            };
        }
    }

    /// <summary>
    /// 禁用统一响应包装特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SkipUnifiedResponseAttribute : Attribute
    {
    }
}
