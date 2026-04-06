using Com.Scm.Enums;

namespace Com.Scm.Sys.Config.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class GetConfigRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmClientTypeEnum client { get; set; }
    }
}
