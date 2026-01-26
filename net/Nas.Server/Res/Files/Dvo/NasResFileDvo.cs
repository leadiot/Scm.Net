using Com.Scm.Dvo;

namespace Com.Scm.Nas.Res.Files.Dvo
{
    /// <summary>
    /// 文档
    /// </summary>
    public class NasResFileDvo : ScmDataDvo
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public NasTypeEnums type { get; set; }

        /// <summary>
        /// 目录ID
        /// </summary>
        public long dir_id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 文档大小
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// 文档摘要
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long ver { get; set; }
    }
}