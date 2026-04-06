namespace Com.Scm.Image.Captcha
{
    /// <summary>
    /// 图形验证码
    /// </summary>
    public interface ICaptcha
    {
        CaptchaResult GenText(CaptchaOption option = null);

        void GenImage(CaptchaResult result);

        CaptchaResult GenCaptcha(CaptchaOption option = null);
    }
}
