using Com.Scm.Otp.Hotp;
using Com.Scm.Utils;
using System;
using System.Text;

namespace Com.Scm.Otp.Totp
{
    /// <summary>
    /// Time-based One-Time Password
    /// TOTP算法（基于时间的一次性密码）实现
    /// 符合RFC 6238标准
    /// https://datatracker.ietf.org/doc/html/rfc6238
    /// </summary>
    public class TotpAuth : OtpAuth
    {
        #region 常量
        /// <summary>
        /// 默认时间步长（秒），推荐使用30秒
        /// </summary>
        public const int DefaultTimeStep = 30;

        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        #endregion

        #region 属性
        /// <summary>
        /// 时间步长（秒）
        /// </summary>
        public int TimeStep { get; private set; }

        /// <summary>
        /// 验证时考虑的时间窗口数量（前后各n个窗口）
        /// </summary>
        public int ValidationWindow { get; set; } = 1;

        /// <summary>
        /// 当前时间
        /// </summary>
        private DateTime _Now;
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
        public override string GenerateCode(string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            _Now = DateTime.UtcNow;

            // 解码Base32密钥
            byte[] keyBytes = TextUtils.Base32Decode(secretKey);

            return GenerateTimeCode(keyBytes, _Now);
        }

        /// <summary>
        /// 生成TOTP密码
        /// </summary>
        /// <param name="secretKey">共享密钥</param>
        /// <returns>生成的一次性密码</returns>
        public override string GenerateCode(byte[] secretKey)
        {
            _Now = DateTime.UtcNow;

            return GenerateTimeCode(secretKey, _Now);
        }

        /// <summary>
        /// 生成指定时间的TOTP密码
        /// </summary>
        /// <param name="secretKey">共享密钥</param>
        /// <param name="utcTime">指定的UTC时间</param>
        /// <returns>生成的一次性密码</returns>
        private string GenerateTimeCode(byte[] secretKey, DateTime utcTime)
        {
            if (utcTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("时间必须是UTC时间", nameof(utcTime));
            }

            // 计算时间计数器
            long counter = GetTimeCounter(utcTime);

            return GenerateCode(secretKey, counter);
        }

        /// <summary>
        /// 验证TOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="code">要验证的密码</param>
        /// <returns>验证结果</returns>
        public override bool VerifyCode(string secretKey, string code)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            // 解码Base32密钥
            byte[] keyBytes = TextUtils.Base32Decode(secretKey);

            return VerifyTimeCode(keyBytes, code, DateTime.UtcNow);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override bool VerifyCode(byte[] secretKey, string code)
        {
            if (secretKey == null || secretKey.Length == 0)
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            return VerifyTimeCode(secretKey, code, DateTime.UtcNow);
        }

        /// <summary>
        /// 验证指定时间的TOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="code">要验证的密码</param>
        /// <param name="utcTime">验证的UTC时间</param>
        /// <returns>验证结果</returns>
        private bool VerifyTimeCode(byte[] secretKey, string code, DateTime utcTime)
        {
            if (utcTime.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException("时间必须是UTC时间", nameof(utcTime));
            }

            // 检查密码长度
            if (code.Length != CodeLength)
            {
                return false;
            }

            // 获取当前时间计数器
            long currentCounter = GetTimeCounter(utcTime);

            // 检查当前窗口及前后ValidationWindow个窗口
            for (int i = -ValidationWindow; i <= ValidationWindow; i++)
            {
                long targetCounter = currentCounter + i;
                string generatedCode = GenerateCode(secretKey, targetCounter);

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
        /// 
        /// </summary>
        /// <returns></returns>
        public override long GetCounter()
        {
            return GetTimeCounter(_Now);
        }

        /// <summary>
        /// 计算指定时间的时间计数器
        /// </summary>
        /// <param name="utcTime">UTC时间</param>
        /// <returns>时间计数器</returns>
        private long GetTimeCounter(DateTime utcTime)
        {
            // 计算总秒数
            long totalSeconds = (long)(utcTime - EPOCH).TotalSeconds;

            // 计算时间计数器
            return GetTimeCounter(totalSeconds);
        }

        /// <summary>
        /// 计算指定时间的时间计数器
        /// </summary>
        /// <param name="seconds">Unix时间戳，以秒为单位</param>
        /// <returns></returns>
        private long GetTimeCounter(long seconds)
        {
            // 计算时间计数器
            return seconds / TimeStep;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool ChangeCounter()
        {
            _Now = DateTime.UtcNow;
            return true;
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

            long totalSeconds = (long)(utcTime - EPOCH).TotalSeconds;
            long remainingSeconds = TimeStep - totalSeconds % TimeStep;

            return utcTime.AddSeconds(remainingSeconds);
        }
        #endregion

        #region 验证方法

        /// <summary>
        /// 验证RFC 4226测试向量
        /// </summary>
        public override bool VerifyRfcTestVectors()
        {
            // RFC 4226附录D的测试向量
            byte[] secretKey = Encoding.UTF8.GetBytes("12345678901234567890");
            long[] counters = { 59, 1111111109, 1111111111, 1234567890, 2000000000, 20000000000 };
            string[] expectedCodes = { "94287082", "07081804", "14050471", "89005924", "69279037", "65353130" };

            HotpAuth hotp = new HotpAuth(30, 8, OtpHashAlgorithm.SHA1);

            for (int i = 0; i < counters.Length; i++)
            {
                string generatedCode = hotp.GenerateCode(secretKey, GetTimeCounter(counters[i]));
                if (generatedCode != expectedCodes[i])
                {
                    Console.WriteLine($"测试失败: 计数器 {counters[i]}，期望 {expectedCodes[i]}，实际 {generatedCode}");
                    return false;
                }
            }

            Console.WriteLine("所有RFC 6238测试向量验证通过");
            return true;
        }

        #endregion

        /// <summary>
        /// 生成TOTP URL（用于生成二维码）
        /// </summary>
        public override string GenerateOtpUrl(OtpConfig config)
        {
            string encodedAccount = Uri.EscapeDataString(config.Account);
            string encodedIssuer = Uri.EscapeDataString(config.Issuer);
            string encodedSecret = Uri.EscapeDataString(TextUtils.Base32Encode(config.Secret));

            return $"otpauth://totp/{encodedIssuer}:{encodedAccount}?secret={encodedSecret}&issuer={encodedIssuer}&algorithm=SHA1&digits=6&period=30";
        }
    }
}
