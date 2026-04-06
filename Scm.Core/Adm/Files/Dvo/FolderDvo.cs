namespace Com.Scm.Adm.Files.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class FolderDvo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 物理路径
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 服务路径
        /// </summary>
        public string uri { get; set; }
        /// <summary>
        /// 子目录
        /// </summary>
        public List<FolderDvo> children { get; set; }
    }
}
