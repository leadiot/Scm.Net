using System;

namespace Com.Scm.Response
{
    public class ScmAppResponse : ScmResponse
    {
        /// <summary>
        /// 处理结果
        /// </summary>
        public bool Success { get { return _rr; } set { _rr = value; } }

        /// <summary>
        /// 返回代码
        /// </summary>
        public int Code { get { return _rc; } set { _rc = value; } }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get { return _rm; } set { _rm = value; } }

        /// <summary>
        /// 响应时间
        /// </summary>
        public long Time { get; set; }

        public static ScmAppResponse SetSuccess<T>(T obj) where T : class
        {
            return new ScmAppDataResponse<T>
            {
                Data = obj,
                Success = true,
                Code = 0,
                Message = "success",
                Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
        }
    }
}
