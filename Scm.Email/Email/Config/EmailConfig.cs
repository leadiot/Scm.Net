using Com.Scm.Config;
using Com.Scm.Utils;

namespace Com.Scm.Email.Config
{
    public class EmailConfig
    {
        public const string NAME = "Email";

        /// <summary>
        /// SMTP服务器地址
        /// </summary>
        public string SmtpServer { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int SmtpPort { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 邮件模板
        /// </summary>
        public string TemplatesDir { get; set; }

        public void Prepare(EnvConfig envConfig)
        {
            if (string.IsNullOrWhiteSpace(TemplatesDir))
            {
                TemplatesDir = "email";
            }
            if (!Path.IsPathRooted(TemplatesDir))
            {
                TemplatesDir = envConfig.GetDataPath(TemplatesDir);
            }
            if (!Directory.Exists(TemplatesDir))
            {
                Directory.CreateDirectory(TemplatesDir);
            }
        }

        /// <summary>
        /// 模板转换
        /// </summary>
        /// <param name="emailHead"></param>
        /// <param name="emailBody"></param>
        /// <param name="emailFoot"></param>
        /// <returns></returns>
        public string GetText(string file, string emailHead, string emailBody, string emailFoot)
        {
            if (string.IsNullOrEmpty(file))
            {
                //return emailHead + emailBody + emailFoot;
                return emailBody;
            }

            var path = Path.Combine(TemplatesDir, file);
            if (!File.Exists(path))
            {
                //return emailHead + emailBody + emailFoot;
                return emailBody;
            }

            var text = FileUtils.ReadText(path);
            if (string.IsNullOrEmpty(text))
            {
                //return emailHead + emailBody + emailFoot;
                return emailBody;
            }

            return text.Replace("{email_head}", emailHead)
                .Replace("{email_body}", emailBody)
                .Replace("{email_foot}", emailFoot);
        }
    }
}