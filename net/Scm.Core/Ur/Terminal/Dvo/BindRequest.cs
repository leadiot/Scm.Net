namespace Com.Scm.Ur.Terminal.Dvo
{
    public class BindRequest : ScmRequest
    {
        public string codes { get; set; }
        public string pass { get; set; }
        public string mac { get; set; }
        public string os { get; set; }
    }
}
