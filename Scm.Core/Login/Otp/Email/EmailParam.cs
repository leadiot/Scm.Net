using Com.Scm.Otp;

namespace Com.Scm.Login.Otp.Email
{
    public class EmailParam : OtpParam
    {
        public string email { get; set; }

        public string template { get; set; }
    }
}
