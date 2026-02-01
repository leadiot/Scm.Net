namespace Com.Scm.Terminal.Dvo
{
    public class BindRequest : ScmRequest
    {
        /// <summary>
        /// 终端代码
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 终端口令
        /// </summary>
        public string pass { get; set; }

        public string mac { get; set; }

        public string os { get; set; }

        public string dn { get; set; }

        public string dm { get; set; }
    }
}
