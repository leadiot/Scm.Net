using System;
using System.Collections.Generic;

namespace Com.Scm.Response
{
    /// <summary>
    /// API 统一响应结果
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// 请求路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ApiResult()
        {
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 成功响应
        /// </summary>
        public static ApiResult Ok(string message = "操作成功")
        {
            return new ApiResult
            {
                Success = true,
                Code = 200,
                Message = message
            };
        }

        /// <summary>
        /// 失败响应
        /// </summary>
        public static ApiResult Error(string message, int code = 500)
        {
            return new ApiResult
            {
                Success = false,
                Code = code,
                Message = message
            };
        }

        /// <summary>
        /// 业务异常响应
        /// </summary>
        public static ApiResult BusinessError(string message, int code = 400)
        {
            return new ApiResult
            {
                Success = false,
                Code = code,
                Message = message
            };
        }
    }

    /// <summary>
    /// API 统一响应结果（带数据）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 成功响应（带数据）
        /// </summary>
        public static ApiResult<T> Ok(T data, string message = "操作成功")
        {
            return new ApiResult<T>
            {
                Success = true,
                Code = 200,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// 成功响应（带分页数据）
        /// </summary>
        public static ApiResult<PagedResult<T>> OkPaged(List<T> items, long total, int page, int pageSize)
        {
            var pagedResult = new PagedResult<T>
            {
                Items = items,
                Total = total,
                Page = page,
                PageSize = pageSize,
                TotalPages = (long)Math.Ceiling((double)total / pageSize)
            };

            return new ApiResult<PagedResult<T>>
            {
                Success = true,
                Code = 200,
                Message = "查询成功",
                Data = pagedResult
            };
        }
    }

    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<T> Items { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPages { get; set; }
    }
}
