using Com.Scm.Enums;
using Com.Scm.Request;

namespace Com.Scm.Sys.Config.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SaveConfigRequest : ScmRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmDataTypeEnum data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmClientTypeEnum client { get; set; }
    }
}
