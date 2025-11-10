namespace Com.Scm.Login.Otp
{
    public class OtpResult
    {
        public string data { get; set; }

        public bool success { get; set; }

        public int error_code { get; set; }
        public string error_message { get; set; }

        public void SetSuccess(string code)
        {
            success = true;
            this.data = code;
        }

        public void SetFailure(int code, string message)
        {
            success = false;
            error_code = code;
            error_message = message;
        }

        public static OtpResult Success(string code)
        {
            return new OtpResult { data = code, success = true };
        }

        public static OtpResult Failure(int code, string message)
        {
            return new OtpResult { error_code = code, error_message = message, success = false };
        }
    }
}
