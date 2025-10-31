using Com.Scm.Dvo;

namespace Com.Scm.Fes.FesApp.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmFesAppDvo : ScmDataDvo
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public long org_id { get; set; }
        public string org_name { get; set; }

        /// <summary>
        /// 应用代码
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string namec { get; set; }
        public string names { get; set; }

        /// <summary>
        /// 应用说明
        /// </summary>
        public string remark { get; set; }
    }
}