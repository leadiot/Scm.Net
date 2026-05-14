namespace Com.Scm.Enums
{
    /// <summary>
    /// API 响应状态码枚举
    /// </summary>
    public enum ResultCodeEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,

        /// <summary>
        /// 参数错误
        /// </summary>
        BadRequest = 400,

        /// <summary>
        /// 未授权
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        /// 禁止访问
        /// </summary>
        Forbidden = 403,

        /// <summary>
        /// 资源不存在
        /// </summary>
        NotFound = 404,

        /// <summary>
        /// 请求方法不允许
        /// </summary>
        MethodNotAllowed = 405,

        /// <summary>
        /// 请求超时
        /// </summary>
        RequestTimeout = 408,

        /// <summary>
        /// 冲突（资源已存在）
        /// </summary>
        Conflict = 409,

        /// <summary>
        /// 请求过于频繁
        /// </summary>
        TooManyRequests = 429,

        /// <summary>
        /// 服务器内部错误
        /// </summary>
        InternalServerError = 500,

        /// <summary>
        /// 服务不可用
        /// </summary>
        ServiceUnavailable = 503,

        /// <summary>
        /// 业务逻辑错误
        /// </summary>
        BusinessError = 1000,

        /// <summary>
        /// 数据验证失败
        /// </summary>
        ValidationError = 1001,

        /// <summary>
        /// 数据不存在
        /// </summary>
        DataNotFound = 1002,

        /// <summary>
        /// 数据已存在
        /// </summary>
        DataAlreadyExists = 1003,

        /// <summary>
        /// 操作失败
        /// </summary>
        OperationFailed = 1004,

        /// <summary>
        /// 第三方服务错误
        /// </summary>
        ThirdPartyError = 2000
    }
}
