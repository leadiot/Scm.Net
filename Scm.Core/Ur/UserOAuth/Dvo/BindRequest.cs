namespace Com.Scm.Ur.UserOAuth.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class BindRequest : ScmUpdateRequest
    {

        public string osp_code { get; set; }
        public string osp_name { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
    }
}
