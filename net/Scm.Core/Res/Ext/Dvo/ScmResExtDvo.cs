using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Res.Ext.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmResExtDvo : ScmDataDvo
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public ScmFileTypeEnum types { get; set; }
        public string types_name { get; set; }

        /// <summary>
        /// 后缀代码
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 后缀名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 文件签名
        /// </summary>
        public string sign { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public long org_id { get; set; }
        public string org_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long app_id { get; set; }
        public string app_name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }


    }
}