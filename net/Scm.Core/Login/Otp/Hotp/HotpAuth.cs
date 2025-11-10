using Com.Scm.Log;
using Com.Scm.Login.Otp;
using System.Security.Cryptography;
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
        #region 属性

        /// <summary>
        /// 
        /// </summary>
        public HotpParam Param { get; private set; }

        /// <summary>
        /// 计数器
        /// </summary>
        private long _Counter;

        #endregion

        #region 构造函数

        /// <summary>
        /// 使用默认参数初始化HOTP实例
        /// </summary>
        public HotpAuth(OtpConfig config) : base(config)
        {
            Type = OtpTypesEnum.Hotp;
        }

        /// <summary>
        /// 参数初始化
        /// </summary>
        /// <param name="param"></param>
        public override bool Init(OtpParam param)
        {
            var tmp = param as HotpParam;

            if (tmp == null)
            {
                tmp = new HotpParam();
                tmp.LoadDefault();
            }

            // 验证参数有效性
            if (tmp.Digits < 4 || tmp.Digits > 8)
            {
                throw new ArgumentOutOfRangeException(nameof(tmp.Digits), "密码长度必须在4-8位之间");
            }

            Param = tmp;
            return true;
        }

        #endregion

        #region 核心方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public override OtpResult GenerateCode(string requestId)
        {
            return GenerateHmacCode(Param.Secret, _Counter);
        }

        /// <summary>
        /// 生成HOTP密码
        /// </summary>
        /// <param name="requestId">Base32编码的共享密钥</param>
        /// <returns>生成的一次性密码</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override Task<OtpResult> GenerateCodeAsync(string requestId)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        public OtpResult GenerateCode(long requestId, long counter)
        {
            return GenerateHmacCode(Param.Secret, counter);
        }

        /// <summary>
        /// 生成HOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="counter">计数器值</param>
        /// <returns>生成的一次性密码</returns>
        private OtpResult GenerateHmacCode(byte[] secretKey, long counter)
        {
            if (counter < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(counter), "计数器不能为负数");
            }

            var code = GenerateCode(secretKey, counter);
            return OtpResult.Success(code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public override OtpResult VerifyCode(string key, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            return VerifyHmacCode(Param.Secret, code, _Counter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public override Task<OtpResult> VerifyCodeAsync(string key, string code)
        {
            return null;
        }

        /// <summary>
        /// 验证HOTP密码
        /// </summary>
        /// <param name="secretKey">Base32编码的共享密钥</param>
        /// <param name="code">要验证的密码</param>
        /// <param name="counter">当前计数器值</param>
        /// <param name="newCounter">验证成功后的新计数器值（输出参数）</param>
        /// <returns>验证结果</returns>
        private OtpResult VerifyHmacCode(byte[] secretKey, string code, long counter)
        {
            var result = new OtpResult();

            if (counter < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(counter), "计数器不能为负数");
            }

            // 检查密码长度
            if (code.Length != Param.Digits)
            {
                result.SetFailure(0, "");
                return result;
            }

            // 获取当前时间计数器
            long currentCounter = counter;

            // 尝试当前计数器及后续ResyncWindow个计数器
            for (int i = 0; i <= Param.Windows; i++)
            {
                long targetCounter = currentCounter + i;
                string generatedCode = GenerateCode(secretKey, targetCounter);

                if (generatedCode == code)
                {
                    // 验证成功，更新计数器
                    //_Counter = targetCounter;
                    result.SetSuccess(generatedCode);
                    return result;
                }
            }

            result.SetFailure(0, "");
            return result;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long GetCounter()
        {
            return _Counter;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetCounter()
        {
            _Counter = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool ChangeCode(string requestId)
        {
            _Counter += Param.HmacStep;
            return true;
        }

        #region 验证方法

        /// <summary>
        /// 验证RFC 4226测试向量
        /// </summary>
        public bool VerifyRfcTestVectors()
        {
            // RFC 4226附录D的测试向量
            byte[] secretKey = Encoding.UTF8.GetBytes("12345678901234567890");
            long[] counters = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            string[] expectedCodes = { "755224", "287082", "359152", "969429", "338314", "254676", "287922", "162583", "399871", "520489" };

            var param = new HotpParam()
            {
                HmacStep = 1,
                Digits = 6,
                Algorithm = HotpAlgorithm.SHA1
            };
            HotpAuth hotp = new HotpAuth(null);
            hotp.Init(param);

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

        #region 私有方法
        /// <summary>
        /// 生成口令
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        protected string GenerateCode(byte[] secretKey, long counter)
        {
            // 生成HMAC哈希
            byte[] hash = ComputeHash(secretKey, counter);

            // 动态截断获取密码
            int code = TruncateHash(hash);

            // 格式化输出
            return code.ToString($"D{Param.Digits}");
        }

        /// <summary>
        /// 创建HMAC实例
        /// </summary>
        /// <param name="keyBytes">密钥字节数组</param>
        /// <returns>HMAC实例</returns>
        protected HMAC CreateInstance(byte[] keyBytes)
        {
            switch (Param.Algorithm)
            {
                case HotpAlgorithm.SHA1:
                    return new HMACSHA1(keyBytes);
                case HotpAlgorithm.SHA256:
                    return new HMACSHA256(keyBytes);
                case HotpAlgorithm.SHA512:
                    return new HMACSHA512(keyBytes);
                default:
                    throw new ArgumentOutOfRangeException(nameof(Param.Algorithm), "不支持的哈希算法");
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
            int mod = (int)Math.Pow(10, Param.Digits);
            return code % mod;
        }

        #endregion

        public override string GenerateOtpUrl(string issuer, string account, byte[] secret)
        {
            return "";
        }
    }
}