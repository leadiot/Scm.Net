namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 知识库文档查询条件
    /// </summary>
    public class SearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 索引状态：0待索引、1索引中、2已索引、3索引失败
        /// </summary>
        public int status { get; set; } = -1;
    }
}
