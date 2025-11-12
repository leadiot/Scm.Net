using Com.Scm.Config;
using Com.Scm.Email.Config;
using Com.Scm.Log;
using Com.Scm.Login.Otp.Hotp;
using Com.Scm.Login.Otp.Totp;
using Com.Scm.Phone.Config;

namespace Com.Scm.Login.Otp
{
    public class OtpConfig
    {
        public const string NAME = "Otp";

        /// <summary>
        /// 
        /// </summary>
        public int Digits { get; set; }

        /// <summary>
        /// 类型：totp,hotp
        /// </summary>
        public OtpTypesEnum Type { get; set; }

        public HotpConfig Hotp { get; set; }

        public TotpConfig Totp { get; set; }

        public PhoneConfig Phone { get; set; }

        public EmailConfig Email { get; set; }

        public void Prepare(EnvConfig envConfig)
        {
            if (Digits < 4 || Digits > 8)
            {
                Digits = 6;
            }

            if (Hotp != null)
            {
                Hotp.Prepare(envConfig);
            }
            if (Totp != null)
            {
                Totp.Prepare(envConfig);
            }
            if (Phone != null)
            {
                Phone.Prepare(envConfig);
            }
            if (Email != null)
            {
                Email.Prepare(envConfig);
            }
        }
    }
}
