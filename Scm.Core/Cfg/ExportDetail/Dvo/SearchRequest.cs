namespace Com.Scm.Cfg.ExportDetail.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long hid { get; set; }

        /// <summary>
        /// 字典栏目Code
        /// </summary>
        public string codec { get; set; }
    }
}
