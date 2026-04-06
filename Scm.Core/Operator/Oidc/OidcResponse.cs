namespace Com.Scm.Operator.Oidc
{
    /// <summary>
    /// OIDC响应基类
    /// </summary>
    public class OidcResponse
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 响应代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 响应提示
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 判断是否成功
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSuccess()
        {
            return Success;
        }

        /// <summary>
        /// 获取响应提示
        /// </summary>
        /// <returns></returns>
        public virtual string GetMessage()
        {
            return Message;
        }

        /// <summary>
        /// 设置成功信息
        /// </summary>
        public virtual void SetSuccess()
        {
            Success = true;
        }

        /// <summary>
        /// 设置成功信息
        /// </summary>
        /// <param name="code"></param>
        public virtual void SetSuccess(int code)
        {
            Success = true;
            Code = code;
        }

        /// <summary>
        /// 设置成功信息
        /// </summary>
        /// <param name="message"></param>
        public virtual void SetSuccess(string message)
        {
            Success = true;
            Message = message;
        }

        /// <summary>
        /// 设置成功信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public virtual void SetSuccess(int code, string message)
        {
            Success = true;
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 设置失败信息
        /// </summary>
        /// <param name="code"></param>
        public virtual void SetFailure(int code)
        {
            Success = false;
            Code = code;
        }

        /// <summary>
        /// 设置失败信息
        /// </summary>
        /// <param name="message"></param>
        public virtual void SetFailure(string message)
        {
            Success = false;
            Message = message;
        }

        /// <summary>
        /// 设置失败信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public virtual void SetFailure(int code, string message)
        {
            Success = false;
            Code = code;
            Message = message;
        }
    }

    /// <summary>
    /// OIDC响应基类（含数据）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class OidcDataResponse<T> : OidcResponse
    {
        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Data { get; set; }

        public void SetSuccess(T data)
        {
            Success = true;
            Code = 0;
            Message = "";
            Data = data;
        }
    }
}
