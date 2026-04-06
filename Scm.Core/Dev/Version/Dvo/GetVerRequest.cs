using Com.Scm.Enums;

namespace Com.Scm.Dev.Version.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class GetVerRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public ScmClientTypeEnum client { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long app_id { get; set; }
    }
}
