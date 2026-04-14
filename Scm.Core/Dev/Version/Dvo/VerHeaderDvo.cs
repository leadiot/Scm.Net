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
        /// 主版本号
        /// </summary>
        public int major { get; set; }

        /// <summary>
        /// 子版本号
        /// </summary>
        public int minor { get; set; }

        /// <summary>
        /// 修订版本号
        /// </summary>
        public int patch { get; set; }

        /// <summary>
        /// 构建版本号，默认自增
        /// </summary>
        public int build { get; set; }

        /// <summary>
        /// 发行日期
        /// </summary>
        public string release_date { get; set; }

        /// <summary>
        /// 发行版本
        /// </summary>
        public string release_code { get; set; }

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
        /// 修选版本
        /// </summary>
        public bool rc { get; set; }

        /// <summary>
        /// 正式版本
        /// </summary>
        public bool ga { get; set; }

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
        /// 下载地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<VerDetailDvo> details { get; set; }
    }
}
