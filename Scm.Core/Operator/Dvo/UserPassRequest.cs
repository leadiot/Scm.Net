namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class UserPassRequest : ScmUpdateRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string OldPass { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string NewPass { get; set; }
    }
}
