using Com.Scm.Request;

namespace Com.Scm.Ur.UserOtp.Dvo
{
    public class VerifyRequest : ScmRequest
    {
        public string code { get; set; }
    }
}
