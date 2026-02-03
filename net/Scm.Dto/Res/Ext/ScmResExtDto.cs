using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Ext
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmResExtDto : ScmDataDto
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public ScmFileTypeEnum types { get; set; }

        /// <summary>
        /// 后缀代码
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }

        /// <summary>
        /// 后缀名称
        /// </summary>
        [StringLength(64)]
        public string namec { get; set; }

        /// <summary>
        /// 文件签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public long org_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long app_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(128)]
        public string mime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        public string remark { get; set; }
    }
}