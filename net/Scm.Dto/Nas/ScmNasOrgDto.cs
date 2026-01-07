using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmNasOrgDto : ScmDataDto
    {
        /// <summary>
        /// 组织代码
        /// </summary>
        [StringLength(16)]
        public string codec { get; set; }

        /// <summary>
        /// 组织简称
        /// </summary>
        [StringLength(32)]
        public string names { get; set; }

        /// <summary>
        /// 组织全称
        /// </summary>
        [StringLength(128)]
        public string namec { get; set; }
    }
}