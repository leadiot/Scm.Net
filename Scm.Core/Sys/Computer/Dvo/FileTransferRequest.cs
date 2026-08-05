using Com.Scm.Request;

namespace Com.Scm.Computer.Dvo
{
    /// <summary>
    /// 文件复制/移动请求
    /// </summary>
    public class FileTransferRequest : ScmRequest
    {
        /// <summary>
        /// 源文件或目录路径
        /// </summary>
        public List<string> src { get; set; }

        /// <summary>
        /// 目标目录路径
        /// </summary>
        public string dst { get; set; }

        /// <summary>
        /// 是否覆盖已存在的目标
        /// </summary>
        public bool overwrite { get; set; }
    }
}
