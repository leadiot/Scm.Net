namespace Com.Scm.Msg.Notice.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public List<long> ids { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SearchFolderEnum folder { get; set; }
    }
}
