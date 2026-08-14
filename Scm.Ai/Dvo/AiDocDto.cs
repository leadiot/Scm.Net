namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 知识库文档
    /// </summary>
    public class AiDocDto
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 文档名称
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string exts { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long file_size { get; set; }

        /// <summary>
        /// 字符数量
        /// </summary>
        public long char_count { get; set; }

        /// <summary>
        /// 片段数量
        /// </summary>
        public int chunk_count { get; set; }

        /// <summary>
        /// 索引状态：0待索引、1索引中、2已索引、3索引失败
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long create_time { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public long create_user { get; set; }
    }
}
