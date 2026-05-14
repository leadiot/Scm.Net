using Com.Scm.Exceptions;
using Com.Scm.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers
{
    /// <summary>
    /// API 统一响应格式演示控制器
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "Scm")]
    public class DemoController : ApiController
    {
        /// <summary>
        /// 成功响应示例（返回简单数据）
        /// </summary>
        [HttpGet("success")]
        public string GetSuccess()
        {
            return "Hello, World!";
        }

        /// <summary>
        /// 成功响应示例（返回对象）
        /// </summary>
        [HttpGet("object")]
        public DemoData GetObject()
        {
            return new DemoData
            {
                Id = 1,
                Name = "测试数据",
                Description = "这是一个演示对象"
            };
        }

        /// <summary>
        /// 成功响应示例（返回列表）
        /// </summary>
        [HttpGet("list")]
        public List<DemoData> GetList()
        {
            return new List<DemoData>
            {
                new DemoData { Id = 1, Name = "数据1", Description = "描述1" },
                new DemoData { Id = 2, Name = "数据2", Description = "描述2" },
                new DemoData { Id = 3, Name = "数据3", Description = "描述3" }
            };
        }

        /// <summary>
        /// 使用 ApiResult 直接返回（已包装格式）
        /// </summary>
        [HttpGet("result")]
        public ApiResult<DemoData> GetResult()
        {
            var data = new DemoData
            {
                Id = 1,
                Name = "测试数据",
                Description = "使用 ApiResult 包装"
            };
            return ApiResult<DemoData>.Ok(data, "查询成功");
        }

        /// <summary>
        /// 分页数据响应示例
        /// </summary>
        [HttpGet("paged")]
        public ApiResult<PagedResult<DemoData>> GetPaged(int page = 1, int pageSize = 10)
        {
            var allData = new List<DemoData>
            {
                new DemoData { Id = 1, Name = "数据1", Description = "描述1" },
                new DemoData { Id = 2, Name = "数据2", Description = "描述2" },
                new DemoData { Id = 3, Name = "数据3", Description = "描述3" },
                new DemoData { Id = 4, Name = "数据4", Description = "描述4" },
                new DemoData { Id = 5, Name = "数据5", Description = "描述5" }
            };

            var pagedData = allData.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return ApiResult<DemoData>.OkPaged(pagedData, allData.Count, page, pageSize);
        }

        /// <summary>
        /// 业务异常示例
        /// </summary>
        [HttpGet("business-error")]
        public IActionResult GetBusinessError()
        {
            throw new BusinessException("这是一个业务逻辑错误示例");
        }

        /// <summary>
        /// 数据不存在异常示例
        /// </summary>
        [HttpGet("not-found")]
        public IActionResult GetNotFound()
        {
            throw new NotFoundException("请求的数据不存在");
        }

        /// <summary>
        /// 验证异常示例
        /// </summary>
        [HttpGet("validation-error")]
        public IActionResult GetValidationError()
        {
            throw new ValidationException("参数验证失败：ID 必须大于 0");
        }

        /// <summary>
        /// 未授权异常示例
        /// </summary>
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            throw new UnauthorizedException("请先登录系统");
        }

        /// <summary>
        /// 禁止访问异常示例
        /// </summary>
        [HttpGet("forbidden")]
        public IActionResult GetForbidden()
        {
            throw new ForbiddenException("您没有权限执行此操作");
        }

        /// <summary>
        /// 服务器内部错误示例
        /// </summary>
        [HttpGet("server-error")]
        public IActionResult GetServerError()
        {
            throw new Exception("服务器内部错误示例");
        }

        /// <summary>
        /// 创建数据示例
        /// </summary>
        [HttpPost("create")]
        public ApiResult<DemoData> Create([FromBody] CreateDemoRequest request)
        {
            // 参数验证
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ValidationException("名称不能为空");
            }

            var data = new DemoData
            {
                Id = new Random().Next(1, 1000),
                Name = request.Name,
                Description = request.Description
            };

            return ApiResult<DemoData>.Ok(data, "创建成功");
        }
    }

    /// <summary>
    /// 演示数据模型
    /// </summary>
    public class DemoData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// 创建演示请求
    /// </summary>
    public class CreateDemoRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
