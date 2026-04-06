namespace Com.Scm.Dev.Sql.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecuteRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long db_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sql { get; set; }
    }
}
