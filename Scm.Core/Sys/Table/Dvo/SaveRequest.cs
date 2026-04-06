using Com.Scm.Request;

namespace Com.Scm.Sys.Table.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SaveRequest : ScmRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SysTableDetailDto> details { get; set; }
    }
}
