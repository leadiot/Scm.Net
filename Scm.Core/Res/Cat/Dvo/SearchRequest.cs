namespace Com.Scm.Res.Cat.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long app_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string app_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long pid { get; set; }
    }
}
