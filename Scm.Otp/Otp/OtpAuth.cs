using System;
using System.Security.Cryptography;

namespace Com.Scm.Otp
{
    /// <summary>
    /// One-Time Password
    /// </summary>
    public abstract class OtpAuth
    {
        #region 常量
        /// <summary>
        /// 默认密码长度（6位数字）
        /// </summary>
        public const int DefaultCodeLength = 6;

        /// <summary>
        /// 默认哈希算法
        /// </summary>
        public const OtpHashAlgorithm DefaultHashAlgorithm = OtpHashAlgorithm.SHA1;
        #endregion

        #region 属性
        /// <summary>
        /// 密码长度（4-8位）
        /// </summary>
        public int CodeLength { get; protected set; }

        /// <summary>
        /// 哈希算法
        /// </summary>
        public OtpHashAlgorithm HashAlgorithm { get; protected set; }
        #endregion

        #region 公共方法

        /// <summary>
        /// 生成校验码
        /// </summary>
        /// <param name="secretKey">Base32编码的密钥</param>
        /// <returns></returns>
        public abstract string GenerateCode(string secretKey);

        /// <summary>
        /// 生成校验码
        /// </summary>
        /// <param name="secretKey">密钥</param>
        /// <returns></returns>
        public abstract string GenerateCode(byte[] secretKey);

        /// <summary>
        /// 验证校验码
        /// </summary>
        /// <param name="secretKey">Base32编码的密钥</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public abstract bool VerifyCode(string secretKey, string code);

        /// <summary>
        /// 验证校验码
        /// </summary>
        /// <param name="secretKey">密钥</param>
        /// <param name="code"></param>
        /// <returns></returns>
        public abstract bool VerifyCode(byte[] secretKey, string code);

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
        /// 生成校验码
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        public virtual string GenerateCode(byte[] secretKey, long counter)
        {
            // 生成HMAC哈希
            byte[] hash = ComputeHash(secretKey, counter);

            // 动态截断获取密码
            int code = TruncateHash(hash);

            // 格式化输出
            return code.ToString($"D{CodeLength}");
        }

        /// <summary>
        /// 生成URL（用于生成二维码）
        /// </summary>
        public virtual string GenerateOtpUrl(OtpConfig config)
        {
            return "";
        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 创建HMAC实例
        /// </summary>
        /// <param name="keyBytes">密钥字节数组</param>
        /// <returns>HMAC实例</returns>
        protected HMAC CreateInstance(byte[] keyBytes)
        {
            switch (HashAlgorithm)
            {
                case OtpHashAlgorithm.SHA1:
                    return new HMACSHA1(keyBytes);
                case OtpHashAlgorithm.SHA256:
                    return new HMACSHA256(keyBytes);
                case OtpHashAlgorithm.SHA512:
                    return new HMACSHA512(keyBytes);
                default:
                    throw new ArgumentOutOfRangeException(nameof(HashAlgorithm), "不支持的哈希算法");
            }
        }

        /// <summary>
        /// 计算HMAC哈希
        /// </summary>
        /// <param name="keyBytes">密钥字节数组</param>
        /// <param name="counter">计数器值</param>
        /// <returns>HMAC哈希结果</returns>
        protected byte[] ComputeHash(byte[] keyBytes, long counter)
        {
            // 将计数器转换为8字节大端序
            byte[] counterBytes = BitConverter.GetBytes(counter);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counterBytes);
            }

            // 根据指定算法创建HMAC实例
            using (HMAC hmac = CreateInstance(keyBytes))
            {
                return hmac.ComputeHash(counterBytes);
            }
        }

        /// <summary>
        /// 动态截断哈希值获取密码
        /// </summary>
        /// <param name="hash">HMAC哈希结果</param>
        /// <returns>密码数字</returns>
        protected int TruncateHash(byte[] hash)
        {
            // 获取偏移量（哈希值最后一个字节的低4位）
            int offset = hash[hash.Length - 1] & 0x0F;

            // 从偏移量开始取4个字节，转换为32位整数
            int code = (hash[offset] & 0x7F) << 24
                     | (hash[offset + 1] & 0xFF) << 16
                     | (hash[offset + 2] & 0xFF) << 8
                     | (hash[offset + 3] & 0xFF);

            // 取模得到指定长度的密码
            int mod = (int)Math.Pow(10, CodeLength);
            return code % mod;
        }

        #endregion

        #region 静态方法
        /// <summary>
        /// 生成随机密钥
        /// </summary>
        /// <param name="length">密钥长度（字节）</param>
        /// <returns>随机密钥</returns>
        public static byte[] GenerateRandomKey(int length = 16)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "长度必须大于0");
            }

            byte[] randomBytes = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return randomBytes;
        }
        #endregion
    }
}
