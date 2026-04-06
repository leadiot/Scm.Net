using Com.Scm.Utils;
using System;
using System.Text;

namespace Com.Scm.Otp.Hotp
{
    /// <summary>
    /// HMAC-based One-Time Password
    /// HOTP算法（基于HMAC的一次性密码）实现
    /// 符合RFC 4226标准
    /// 文档：https://datatracker.ietf.org/doc/html/rfc4226
    /// </summary>
    public class HotpAuth : OtpAuth
    {
        #region 常量
        /// <summary>
        /// 默认时间步长（秒），推荐使用30秒
        /// </summary>
        public const int DefaultHmacStep = 3;
        #endregion

        #region 属性
        /// <summary>
        /// 时间步长（秒）
        /// </summary>
        public int HmacStep { get; private set; }

        /// <summary>
        /// 重新同步参数s，表示验证时尝试的计数器偏移量
        /// </summary>
        public int ResyncWindow { get; set; } = 2;

        /// <summary>
        /// 计数器
        /// </summary>
        private long _Counter;
        #endregion

        #region 构造函数

        /// <summary>
        /// 使用默认参数初始化HOTP实例
        /// </summary>
        public HotpAuth() : this(DefaultHmacStep, DefaultCodeLength, DefaultHashAlgorithm)
        {
        }

        /// <summary>
        /// 使用指定参数初始化HOTP实例
        /// </summary>
        /// <param name="codeLength">密码长度（4-8位）</param>
        /// <param name="hashAlgorithm">哈希算法</param>
        public HotpAuth(int hmacStep, int codeLength, OtpHashAlgorithm hashAlgorithm)
        {
            // 验证参数有效性
            if (codeLength < 4 || codeLength > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(codeLength), "密码长度必须在4-8位之间");
            }

            HmacStep = hmacStep;
            CodeLength = codeLength;
            HashAlgorithm = hashAlgorithm;
        }

        #endregion

        #region 核心方法

        /// <summary>
        /// 生成HOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <returns>生成的一次性密码</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override string GenerateCode(string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            // 解码Base32密钥
            byte[] keyBytes = TextUtils.Base32Decode(secretKey);
            return GenerateHmacCode(keyBytes, _Counter);
        }

        /// <summary>
        /// 生成HOTP密码
        /// </summary>
        /// <param name="secretKey">共享密钥</param>
        /// <returns>生成的一次性密码</returns>
        public override string GenerateCode(byte[] secretKey)
        {
            if (secretKey == null || secretKey.Length == 0)
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            return GenerateHmacCode(secretKey, _Counter);
        }

        /// <summary>
        /// 生成HOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="counter">计数器值</param>
        /// <returns>生成的一次性密码</returns>
        private string GenerateHmacCode(byte[] secretKey, long counter)
        {
            if (counter < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(counter), "计数器不能为负数");
            }

            return GenerateCode(secretKey, counter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
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

            return VerifyHmacCode(keyBytes, code, _Counter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
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

            return VerifyHmacCode(secretKey, code, _Counter);
        }

        /// <summary>
        /// 验证HOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="code">要验证的密码</param>
        /// <param name="counter">当前计数器值</param>
        /// <param name="newCounter">验证成功后的新计数器值（输出参数）</param>
        /// <returns>验证结果</returns>
        private bool VerifyHmacCode(byte[] secretKey, string code, long counter)
        {
            if (counter < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(counter), "计数器不能为负数");
            }

            // 检查密码长度
            if (code.Length != CodeLength)
            {
                return false;
            }

            // 获取当前时间计数器
            long currentCounter = counter;

            // 尝试当前计数器及后续ResyncWindow个计数器
            for (int i = 0; i <= ResyncWindow; i++)
            {
                long targetCounter = currentCounter + i;
                string generatedCode = GenerateCode(secretKey, targetCounter);

                if (generatedCode == code)
                {
                    // 验证成功，更新计数器
                    //_Counter = targetCounter;
                    return true;
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override long GetCounter()
        {
            return _Counter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool ChangeCounter()
        {
            _Counter += HmacStep;
            return true;
        }

        #region 验证方法

        /// <summary>
        /// 验证RFC 4226测试向量
        /// </summary>
        public override bool VerifyRfcTestVectors()
        {
            // RFC 4226附录D的测试向量
            byte[] secretKey = Encoding.UTF8.GetBytes("12345678901234567890");
            long[] counters = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string[] expectedCodes = { "755224", "287082", "359152", "969429", "338314", "254676", "287922", "162583", "399871", "520489" };

            HotpAuth hotp = new HotpAuth(1, 6, OtpHashAlgorithm.SHA1);

            for (int i = 0; i < counters.Length; i++)
            {
                string generatedCode = hotp.GenerateCode(secretKey, counters[i]);
                if (generatedCode != expectedCodes[i])
                {
                    Console.WriteLine($"测试失败: 计数器 {counters[i]}，期望 {expectedCodes[i]}，实际 {generatedCode}");
                    return false;
                }
            }

            Console.WriteLine("所有RFC 4226测试向量验证通过");
            return true;
        }

        #endregion
    }
}