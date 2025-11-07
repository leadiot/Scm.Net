namespace Com.Scm.Otp
{
    /// <summary>
    /// One-Time Password
    /// </summary>
    public abstract class OtpAuth
    {
        #region 属性
        /// <summary>
        /// 随机口令类型
        /// </summary>
        public OtpType Type { get; set; }
        #endregion

        #region 公共方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public abstract void Init(OtpParam param);

        /// <summary>
        /// 生成口令
        /// </summary>
        /// <param name="requestId">请求标识</param>
        /// <returns></returns>
        public abstract string GenerateCode(long requestId);

        /// <summary>
        /// 验证口令
        /// </summary>
        /// <param name="requestId">请求标识</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public abstract bool VerifyCode(long requestId, string code);

        /// <summary>
        /// 获取计数器
        /// </summary>
        /// <returns></returns>
        public abstract long GetCounter();

        /// <summary>
        /// 更新计数器
        /// </summary>
        /// <returns></returns>
        public abstract bool ChangeCounter();

        /// <summary>
        /// RFC标准验证
        /// </summary>
        /// <returns></returns>
        public abstract bool VerifyRfcTestVectors();
        #endregion

        #region 虚方法
        /// <summary>
        /// 生成URL（用于生成二维码）
        /// </summary>
        public virtual string GenerateOtpUrl(string issuer, string account, byte[] secret)
        {
            return "";
        }
        #endregion
    }
}
