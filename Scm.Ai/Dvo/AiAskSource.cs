namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 知识库引用来源
    /// </summary>
    public class AiAskSource
    {
        /// <summary>
        /// 文档ID
        /// </summary>
        public long doc_id { get; set; }

        /// <summary>
        /// 文档名称
        /// </summary>
        public string doc_name { get; set; }

        /// <summary>
        /// 片段序号
        /// </summary>
        public int chunk_no { get; set; }

        /// <summary>
        /// 相似度得分
        /// </summary>
        public double score { get; set; }

        /// <summary>
        /// 片段内容
        /// </summary>
        public string content { get; set; }
    }
}
