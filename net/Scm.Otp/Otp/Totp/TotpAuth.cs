using Com.Scm.Utils;
using System;

namespace Com.Scm.Otp.Totp
{
    /// <summary>
    /// Time based One-Time Password
    /// TOTP算法（基于时间的一次性密码）实现
    /// 符合RFC 6238标准
    /// </summary>
    public class TotpAuth : OtpAuth
    {
        #region 常量定义
        /// <summary>
        /// 默认时间步长（秒），推荐使用30秒
        /// </summary>
        public const int DefaultTimeStep = 30;
        #endregion

        #region 属性
        /// <summary>
        /// 时间步长（秒）
        /// </summary>
        public int TimeStep { get; }

        /// <summary>
        /// 验证时考虑的时间窗口数量（前后各n个窗口）
        /// </summary>
        public int ValidationWindow { get; set; } = 1;
        #endregion

        #region 构造函数

        /// <summary>
        /// 使用默认参数初始化TOTP实例
        /// </summary>
        public TotpAuth() : this(DefaultTimeStep, DefaultCodeLength, DefaultHashAlgorithm)
        {
        }

        /// <summary>
        /// 使用指定参数初始化TOTP实例
        /// </summary>
        /// <param name="timeStep">时间步长（秒）</param>
        /// <param name="codeLength">密码长度（4-8位）</param>
        /// <param name="hashAlgorithm">哈希算法</param>
        public TotpAuth(int timeStep, int codeLength, OtpHashAlgorithm hashAlgorithm)
        {
            // 验证参数有效性
            if (timeStep <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(timeStep), "时间步长必须大于0");
            }

            if (codeLength < 4 || codeLength > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(codeLength), "密码长度必须在4-8位之间");
            }

            TimeStep = timeStep;
            CodeLength = codeLength;
            HashAlgorithm = hashAlgorithm;
        }
        #endregion

        #region 核心方法
        /// <summary>
        /// 生成TOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <returns>生成的一次性密码</returns>
        public string GenerateCode(string secretKey)
        {
            return GenerateCode(secretKey, DateTime.UtcNow);
        }

        /// <summary>
        /// 生成指定时间的TOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="utcTime">指定的UTC时间</param>
        /// <returns>生成的一次性密码</returns>
        public string GenerateCode(string secretKey, DateTime utcTime)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            if (utcTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("时间必须是UTC时间", nameof(utcTime));
            }

            // 解码Base32密钥
            byte[] keyBytes = TextUtils.Base32Decode(secretKey);

            // 计算时间计数器
            long counter = GetTimeCounter(utcTime);

            return GenerateCode(keyBytes, counter);
        }

        /// <summary>
        /// 验证TOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="code">要验证的密码</param>
        /// <returns>验证结果</returns>
        public bool VerifyCode(string secretKey, string code)
        {
            return VerifyCode(secretKey, code, DateTime.UtcNow);
        }

        /// <summary>
        /// 验证指定时间的TOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="code">要验证的密码</param>
        /// <param name="utcTime">验证的UTC时间</param>
        /// <returns>验证结果</returns>
        public bool VerifyCode(string secretKey, string code, DateTime utcTime)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (utcTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("时间必须是UTC时间", nameof(utcTime));
            }

            // 检查密码长度
            if (code.Length != CodeLength)
            {
                return false;
            }

            // 解码Base32密钥
            byte[] keyBytes = TextUtils.Base32Decode(secretKey);

            // 获取当前时间计数器
            long currentCounter = GetTimeCounter(utcTime);

            // 检查当前窗口及前后ValidationWindow个窗口
            for (int i = -ValidationWindow; i <= ValidationWindow; i++)
            {
                long targetCounter = currentCounter + i;
                string generatedCode = GenerateCode(keyBytes, targetCounter);

                if (generatedCode == code)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region 辅助方法
        /// <summary>
        /// 计算指定时间的时间计数器
        /// </summary>
        /// <param name="utcTime">UTC时间</param>
        /// <returns>时间计数器</returns>
        private long GetTimeCounter(DateTime utcTime)
        {
            // Unix时间戳起始时间：1970-01-01 00:00:00 UTC
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // 计算总秒数
            long totalSeconds = (long)(utcTime - epoch).TotalSeconds;

            // 计算时间计数器
            return totalSeconds / TimeStep;
        }

        #endregion

        #region 静态辅助方法
        /// <summary>
        /// 计算密码过期时间
        /// </summary>
        /// <param name="utcTime">当前UTC时间</param>
        /// <returns>密码过期时间</returns>
        public DateTime GetExpirationTime(DateTime utcTime)
        {
            if (utcTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("时间必须是UTC时间", nameof(utcTime));
            }

            long totalSeconds = (long)(utcTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            long remainingSeconds = TimeStep - totalSeconds % TimeStep;

            return utcTime.AddSeconds(remainingSeconds);
        }
        #endregion
    }
}
