using Com.Scm.Utils;
using System;

namespace Com.Scm.Otp.Hotp
{
    /// <summary>
    /// HMAC based One-Time Password
    /// HOTP算法（基于HMAC的一次性密码）实现
    /// 符合RFC 4226标准
    /// </summary>
    public class HotpAuth : OtpAuth
    {
        #region 属性

        /// <summary>
        /// 重新同步参数s，表示验证时尝试的计数器偏移量
        /// </summary>
        public int ResyncWindow { get; set; } = 2;

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用默认参数初始化HOTP实例
        /// </summary>
        public HotpAuth() : this(DefaultCodeLength, DefaultHashAlgorithm)
        {
        }

        /// <summary>
        /// 使用指定参数初始化HOTP实例
        /// </summary>
        /// <param name="codeLength">密码长度（4-8位）</param>
        /// <param name="hashAlgorithm">哈希算法</param>
        public HotpAuth(int codeLength, OtpHashAlgorithm hashAlgorithm)
        {
            // 验证参数有效性
            if (codeLength < 4 || codeLength > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(codeLength), "密码长度必须在4-8位之间");
            }

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
            return GenerateCode(secretKey, 0);
        }

        /// <summary>
        /// 生成HOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="counter">计数器值</param>
        /// <returns>生成的一次性密码</returns>
        public string GenerateCode(string secretKey, long counter)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            if (counter < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(counter), "计数器不能为负数");
            }

            // 解码Base32密钥
            byte[] keyBytes = TextUtils.Base32Decode(secretKey);

            // 生成HMAC哈希
            byte[] hash = ComputeHmacHash(keyBytes, counter);

            // 动态截断获取密码
            int code = TruncateHash(hash);

            // 格式化输出
            return code.ToString($"D{CodeLength}");
        }

        /// <summary>
        /// 验证HOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="code">要验证的密码</param>
        /// <param name="currentCounter">当前计数器值</param>
        /// <param name="newCounter">验证成功后的新计数器值（输出参数）</param>
        /// <returns>验证结果</returns>
        public bool VerifyCode(string secretKey, string code, long currentCounter, out long newCounter)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException(nameof(secretKey));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (currentCounter < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentCounter), "计数器不能为负数");
            }

            newCounter = currentCounter;

            // 检查密码长度
            if (code.Length != CodeLength)
            {
                return false;
            }

            // 尝试当前计数器及后续ResyncWindow个计数器
            for (int i = 0; i <= ResyncWindow; i++)
            {
                long targetCounter = currentCounter + i;
                string generatedCode = GenerateCode(secretKey, targetCounter);

                if (generatedCode == code)
                {
                    // 验证成功，更新计数器
                    newCounter = targetCounter + 1;
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region 静态辅助方法

        /// <summary>
        /// 验证RFC 4226测试向量
        /// </summary>
        public static bool VerifyRfcTestVectors()
        {
            // RFC 4226附录D的测试向量
            byte[] random = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            var secret = TextUtils.Base32Encode(random);
            long[] counters = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string[] expectedCodes = { "755224", "287082", "359152", "969429", "338314", "254676", "287922", "162583", "399871", "520489" };

            HotpAuth hotp = new HotpAuth(6, OtpHashAlgorithm.SHA1);

            for (int i = 0; i < counters.Length; i++)
            {
                string generatedCode = hotp.GenerateCode(secret, counters[i]);
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