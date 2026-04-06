using Com.Scm.Log;
using Com.Scm.Login.Otp;

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
        public OtpTypesEnum Type { get; protected set; }

        /// <summary>
        /// 参数配置
        /// </summary>
        public OtpConfig Config { get; protected set; }
        #endregion

        #region 构造器
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="config"></param>
        public OtpAuth(OtpConfig config)
        {
            Config = config;
        }
        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="param"></param>
        public abstract bool Init(OtpParam param);

        /// <summary>
        /// 生成口令（同步）
        /// </summary>
        /// <param name="requestId">请求标识</param>
        /// <returns></returns>
        public abstract OtpResult GenerateCode(string requestId);

        /// <summary>
        /// 生成口令（异步）
        /// </summary>
        /// <param name="requestId">请求标识</param>
        /// <returns></returns>
        public abstract Task<OtpResult> GenerateCodeAsync(string requestId);

        /// <summary>
        /// 验证口令（同步）
        /// </summary>
        /// <param name="key">校验凭证</param>
        /// <param name="code">校验码</param>
        /// <returns></returns>
        public abstract OtpResult VerifyCode(string key, string code);

        /// <summary>
        /// 验证口令（异步）
        /// </summary>
        /// <param name="key">校验凭证</param>
        /// <param name="code">校验码</param>
        /// <returns></returns>
        public abstract Task<OtpResult> VerifyCodeAsync(string key, string code);

        /// <summary>
        /// 更新口令
        /// </summary>
        /// <returns></returns>
        public abstract bool ChangeCode(string requestId);

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
