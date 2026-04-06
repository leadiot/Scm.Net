namespace Com.Scm.Adm.Files.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class FileDvo
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
        /// 文件大小
        /// </summary>
        public long size { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessTime { get; set; }
    }
}
