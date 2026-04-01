namespace Com.Scm.Response
{
    public class ScmResponse
    {
        /// <summary>
        /// 结果标识
        /// </summary>
        protected bool _rr;
        /// <summary>
        /// 返回代码
        /// </summary>
        protected int _rc;
        /// <summary>
        /// 返回消息
        /// </summary>
        protected string _rm;

        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public virtual bool IsSuccess()
        {
            return _rr;
        }

        /// <summary>
        /// 获取返回消息
        /// </summary>
        /// <returns></returns>
        public virtual string GetMessage()
        {
            return _rm;
        }

        /// <summary>
        /// 设置为成功
        /// </summary>
        public virtual void SetSuccess()
        {
            _rr = true;
        }

        /// <summary>
        /// 设置为成功，并指定返回代码
        /// </summary>
        /// <param name="code"></param>
        public virtual void SetSuccess(int code)
        {
            _rr = true;
            _rc = code;
        }

        /// <summary>
        /// 设置为成功，并指定返回消息
        /// </summary>
        /// <param name="message"></param>
        public virtual void SetSuccess(string message)
        {
            _rr = true;
            _rm = message;
        }

        /// <summary>
        /// 设置为成功，并指定返回代码和返回消息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public virtual void SetSuccess(int code, string message)
        {
            _rr = true;
            _rc = code;
            _rm = message;
        }

        /// <summary>
        /// 设置为失败，并指定返回代码
        /// </summary>
        /// <param name="code"></param>
        public virtual void SetFailure(int code)
        {
            _rr = false;
            _rc = code;
        }

        /// <summary>
        /// 设置为失败，并指定返回消息
        /// </summary>
        /// <param name="message"></param>
        public virtual void SetFailure(string message)
        {
            _rr = false;
            _rm = message;
        }

        /// <summary>
        /// 设置为失败，并指定返回代码和返回消息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public virtual void SetFailure(int code, string message)
        {
            _rr = false;
            _rc = code;
            _rm = message;
        }
    }
}
