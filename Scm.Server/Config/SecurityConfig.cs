using Microsoft.AspNetCore.Hosting;

namespace Com.Scm.Config
{
    public class SecurityConfig
    {
        public const string NAME = "Security";

        /// <summary>
        /// 应用编号
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// Aes加密密钥，暂时没有使用
        /// </summary>
        public string AesKey { get; set; }

        /// <summary>
        /// Des加密密钥，暂时没有使用
        /// </summary>
        public string DesKey { get; set; }

        /// <summary>
        /// 前端约定签名值，暂时没有使用
        /// </summary>
        public string SignKey { get; set; }

        /// <summary>
        /// 是否强制检验
        /// </summary>
        public bool CheckSignature { get; set; }

        /// <summary>
        /// 是否限制IP
        /// </summary>
        public bool CheckApp { get; set; }

        public void Prepare(IWebHostEnvironment environment)
        {
        }
    }
}
