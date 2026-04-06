using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Dev.Version.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class VerHeaderDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public ScmClientTypeEnum client { get; set; }

        /// <summary>
        /// 系统
        /// </summary>
        public long app_id { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string app_name { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public string ver { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// 构建版本
        /// </summary>
        public string build { get; set; }

        /// <summary>
        /// 最小版本
        /// </summary>
        public string ver_min { get; set; }
        /// <summary>
        /// 最大版本
        /// </summary>
        public string ver_max { get; set; }

        /// <summary>
        /// 是否内测
        /// </summary>
        public bool alpha { get; set; }

        /// <summary>
        /// 是否公测
        /// </summary>
        public bool beta { get; set; }

        /// <summary>
        /// 强制更新
        /// </summary>
        public bool forced { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public bool current { get; set; }

        /// <summary>
        /// 更新事项
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<VerDetailDvo> details { get; set; }
    }
}
