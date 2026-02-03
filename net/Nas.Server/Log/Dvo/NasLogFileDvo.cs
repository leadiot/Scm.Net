using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Nas.Log
{
    /// <summary>
    /// 同步日志
    /// </summary>
    public class NasLogFileDvo : ScmDataDvo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 终端ID
        /// </summary>
        public long terminal_id { get; set; }

        /// <summary>
        /// 驱动ID
        /// </summary>
        public long drive_id { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public ScmFileTypeEnum type { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public NasOptEnums opt { get; set; }

        /// <summary>
        /// 同步方向
        /// </summary>
        public NasDirEnums dir { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 文件摘要
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        /// 来源文件
        /// </summary>
        public string src { get; set; }
    }
}