using Com.Scm.Enums;
using Com.Scm.Request;

namespace Com.Scm.Adm.Files.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ListFileRequest : ScmRequest
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public ScmFileKindEnum kind { get; set; }
    }
}
