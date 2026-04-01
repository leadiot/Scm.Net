using Com.Scm.Response;

namespace Com.Scm
{
    public class ScmDownloadResponse : ScmApiResponse
    {
        /// <summary>
        /// 文件大小
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// 文件摘要
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string path { get; set; }
    }
}
