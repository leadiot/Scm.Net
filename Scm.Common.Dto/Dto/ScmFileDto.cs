namespace Com.Scm.Dto
{
    public class ScmFileDto : ScmDto
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 完整路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 文件图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 文件摘要（MD5）
        /// </summary>
        public string hash { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public long change_time { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long create_time { get; set; }

        public bool is_dir { get; set; }
    }
}
