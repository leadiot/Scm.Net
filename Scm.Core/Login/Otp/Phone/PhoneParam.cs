using Com.Scm.Otp;

namespace Com.Scm.Login.Otp.Phone
{
    public class PhoneParam : OtpParam
    {
        public string phone { get; set; }

        public string template { get; set; }
    }
}
