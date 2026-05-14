using Com.Scm.Enums;
using System;

namespace Com.Scm.Exceptions
{
    /// <summary>
    /// API 异常基类
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiException(string message, int code = 500) : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiException(string message, ResultCodeEnum code) : base(message)
        {
            Code = (int)code;
        }
    }

    /// <summary>
    /// 业务异常
    /// </summary>
    public class BusinessException : ApiException
    {
        public BusinessException(string message) : base(message, ResultCodeEnum.BusinessError)
        {
        }

        public BusinessException(string message, int code) : base(message, code)
        {
        }
    }

    /// <summary>
    /// 数据验证异常
    /// </summary>
    public class ValidationException : ApiException
    {
        public ValidationException(string message) : base(message, ResultCodeEnum.ValidationError)
        {
        }
    }

    /// <summary>
    /// 数据不存在异常
    /// </summary>
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message = "数据不存在") : base(message, ResultCodeEnum.DataNotFound)
        {
        }
    }

    /// <summary>
    /// 未授权异常
    /// </summary>
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message = "未授权访问") : base(message, ResultCodeEnum.Unauthorized)
        {
        }
    }

    /// <summary>
    /// 禁止访问异常
    /// </summary>
    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message = "禁止访问") : base(message, ResultCodeEnum.Forbidden)
        {
        }
    }
}
