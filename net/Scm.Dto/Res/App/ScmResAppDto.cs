using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.App
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmResAppDto : ScmDataDto
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public long org_id { get; set; }

        /// <summary>
        /// 应用代码
        /// </summary>
        [StringLength(16)]
        public string codec { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [StringLength(64)]
        public string namec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(64)]
        public string names { get; set; }

        /// <summary>
        /// 应用说明
        /// </summary>
        [StringLength(128)]
        public string remark { get; set; }
    }
}