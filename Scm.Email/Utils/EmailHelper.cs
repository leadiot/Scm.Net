using Com.Scm.Email.Config;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Com.Scm.Utils
{
    /// <summary>
    /// 发送邮箱
    /// </summary>
    [Serializable]
    public static class EmailHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="receiver">收件人信息</param>
        /// <returns></returns>
        public static bool SendEmail(EmailConfig config,
            string subject,
            string body,
            EmailAddress receiver)
        {
            var message = new MimeMessage();
            //发件人
            message.From.Add(new MailboxAddress(config.Sender, config.Username));
            //收件人
            receiver.Trim();
            message.To.Add(new MailboxAddress(receiver.Name, receiver.Address));
            //标题
            message.Subject = subject;
            //生成一个支持Html的TextPart
            var part = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            //创建Multipart添加附件
            Multipart multipart = new Multipart("mixed");
            multipart.Add(part);

            //正文
            message.Body = multipart;

            using (var client = new SmtpClient())
            {
                //Smtp服务器
                client.Connect(config.SmtpServer, config.SmtpPort, true);
                if (client.IsConnected)
                {
                    //登录
                    client.Authenticate(config.Username, config.Password);
                    //发送
                    string result = client.Send(message);
                }

                //断开
                client.Disconnect(true);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="receiver">收件人信息</param>
        /// <returns></returns>
        public static async Task<bool> SendEmailAsync(EmailConfig config,
            string subject,
            string body,
            EmailAddress receiver)
        {
            var message = new MimeMessage();
            //发件人
            message.From.Add(new MailboxAddress(config.Sender, config.Username));
            //收件人
            receiver.Trim();
            message.To.Add(new MailboxAddress(receiver.Name, receiver.Address));
            //标题
            message.Subject = subject;
            //生成一个支持Html的TextPart
            var part = new TextPart(TextFormat.Html)
            {
                Text = body
            };

            //创建Multipart添加附件
            Multipart multipart = new Multipart("mixed");
            multipart.Add(part);

            //正文
            message.Body = multipart;

            using (var client = new SmtpClient())
            {
                //Smtp服务器
                await client.ConnectAsync(config.SmtpServer, config.SmtpPort, true);
                if (client.IsConnected)
                {
                    //登录
                    await client.AuthenticateAsync(config.Username, config.Password);
                    //发送
                    string result = await client.SendAsync(message);
                }

                //断开
                await client.DisconnectAsync(true);
            }

            return true;
        }
    }

    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public void Trim()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = Address;
            }
        }
    }
}
