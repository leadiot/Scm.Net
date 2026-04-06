using Com.Scm.Config;

namespace Com.Scm.Phone.Config
{
    public class PhoneConfig
    {
        public const string NAME = "Phone";

        public string Username { get; set; }
        public string Password { get; set; }
        public string SignCode { get; set; }
        public string SignName { get; set; }
        public string TemplateCode { get; set; }
        public string TemplateData { get; set; }

        public void Prepare(EnvConfig envConfig)
        {
        }

        public string GetText(string text)
        {
            return TemplateData.Replace("$sms$", text);
        }
    }
}
