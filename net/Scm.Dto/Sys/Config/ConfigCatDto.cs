using Com.Scm.Dto;
using Com.Scm.Enums;

namespace Com.Scm.Sys.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigCatDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long pid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string namec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int od { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmRowSystemEnum row_system { get; set; } = ScmRowSystemEnum.No;
    }
}
