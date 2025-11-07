using Com.Scm.Utils;
using System.Security.Cryptography;
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
        /// Unix时间基点
        /// </summary>
        private static readonly DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        #endregion

        #region 属性

        /// <summary>
        /// 配置参数
        /// </summary>
        private TotpParam _Param;

        /// <summary>
        /// 当前时间
        /// </summary>
        private DateTime _Now;
        #endregion

        #region 构造函数

        /// <summary>
        /// 使用默认参数初始化TOTP实例
        /// </summary>
        public TotpAuth()
        {
        }

        /// <summary>
        /// 参数初始化
        /// </summary>
        /// <param name="param"></param>
        public override void Init(OtpParam param)
        {
            var tmp = param as TotpParam;

            if (tmp == null)
            {
                tmp = new TotpParam();
                tmp.LoadDefault();
            }

            _Param = tmp;
        }
        #endregion

        #region 核心方法

        /// <summary>
        /// 生成TOTP密码
        /// </summary>
        /// <param name="requestId">Base32编码的共享密钥</param>
        /// <returns>生成的一次性密码</returns>
        public override string GenerateCode(long requestId)
        {
            _Now = DateTime.UtcNow;

            return GenerateTimeCode(_Param.Secret, _Now);
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
        /// <param name="requestId">Base32编码的共享密钥</param>
        /// <param name="code">要验证的密码</param>
        /// <returns>验证结果</returns>
        public override bool VerifyCode(long requestId, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            return VerifyTimeCode(_Param.Secret, code, DateTime.UtcNow);
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
            if (code.Length != _Param.Digits)
            {
                return false;
            }

            // 获取当前时间计数器
            long currentCounter = GetTimeCounter(utcTime);

            // 检查当前窗口及前后ValidationWindow个窗口
            for (int i = -_Param.Windows; i <= _Param.Windows; i++)
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
            return seconds / _Param.TimeStep;
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
            long remainingSeconds = _Param.TimeStep - totalSeconds % _Param.TimeStep;

            return utcTime.AddSeconds(remainingSeconds);
        }
        #endregion

        #region 验证方法

        /// <summary>
        /// 验证RFC6238测试向量
        /// </summary>
        public override bool VerifyRfcTestVectors()
        {
            // RFC6238附录的测试向量
            byte[] secretKey = Encoding.UTF8.GetBytes("12345678901234567890");
            long[] counters = { 59, 1111111109, 1111111111, 1234567890, 2000000000, 20000000000 };
            string[] expectedCodes = { "94287082", "07081804", "14050471", "89005924", "69279037", "65353130" };

            var param = new TotpParam
            {
                TimeStep = 30,
                Digits = 8,
                Algorithm = TotpAlgorithm.SHA1
            };
            TotpAuth hotp = new TotpAuth();
            hotp.Init(param);

            for (int i = 0; i < counters.Length; i++)
            {
                string generatedCode = hotp.GenerateCode(secretKey, hotp.GetTimeCounter(counters[i]));
                if (generatedCode != expectedCodes[i])
                {
                    Console.WriteLine($"测试失败: 计数器 {counters[i]}，期望 {expectedCodes[i]}，实际 {generatedCode}");
                    return false;
                }
            }

            Console.WriteLine("所有RFC6238测试向量验证通过");
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
            return code.ToString($"D{_Param.Digits}");
        }

        /// <summary>
        /// 创建HMAC实例
        /// </summary>
        /// <param name="keyBytes">密钥字节数组</param>
        /// <returns>HMAC实例</returns>
        protected HMAC CreateInstance(byte[] keyBytes)
        {
            switch (_Param.Algorithm)
            {
                case TotpAlgorithm.SHA1:
                    return new HMACSHA1(keyBytes);
                case TotpAlgorithm.SHA256:
                    return new HMACSHA256(keyBytes);
                case TotpAlgorithm.SHA512:
                    return new HMACSHA512(keyBytes);
                default:
                    throw new ArgumentOutOfRangeException(nameof(_Param.Algorithm), "不支持的哈希算法");
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
            int mod = (int)Math.Pow(10, _Param.Digits);
            return code % mod;
        }

        #endregion

        /// <summary>
        /// 生成TOTP URL（用于生成二维码）
        /// </summary>
        public override string GenerateOtpUrl(string issuer, string account, byte[] secret)
        {
            string encodedIssuer = Uri.EscapeDataString(issuer);
            string encodedAccount = Uri.EscapeDataString(account);
            string encodedSecret = Uri.EscapeDataString(TextUtils.Base32Encode(secret));
            string algorithm = Enum.GetName(typeof(TotpAlgorithm), _Param.Algorithm);

            return $"otpauth://totp/{encodedIssuer}:{encodedAccount}?secret={encodedSecret}&issuer={encodedIssuer}&algorithm={algorithm}&digits={_Param.Digits}&period={_Param.TimeStep}";
        }
    }
}
