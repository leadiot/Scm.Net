using Com.Scm.Request;

namespace Com.Scm.Computer.Dvo
{
    /// <summary>
    /// 文件删除请求
    /// </summary>
    public class FileDeleteRequest : ScmRequest
    {
        /// <summary>
        /// 要删除的文件或目录路径
        /// </summary>
        public List<string> path { get; set; }
    }
}
