using Com.Scm.Request;

namespace Com.Scm.Computer.Dvo
{
    /// <summary>
    /// 文件重命名请求
    /// </summary>
    public class FileRenameRequest : ScmRequest
    {
        /// <summary>
        /// 源文件或目录路径
        /// </summary>
        public string src { get; set; }

        /// <summary>
        /// 新的名称（不含路径）
        /// </summary>
        public string name { get; set; }
    }
}
