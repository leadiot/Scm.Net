using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 
    /// </summary>
    public class SysThemeDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        [StringLength(32)]
        public string names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(1024)]
        public string theme { get; set; }
    }
}