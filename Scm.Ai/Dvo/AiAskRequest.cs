namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 知识库问答请求（RAG）
    /// </summary>
    public class AiAskRequest
    {
        /// <summary>
        /// 问题内容
        /// </summary>
        public string question { get; set; }

        /// <summary>
        /// 知识库文档ID，0表示全部文档
        /// </summary>
        public long doc_id { get; set; }

        /// <summary>
        /// 检索返回的片段数量，0表示使用配置默认值
        /// </summary>
        public int top_k { get; set; }

        /// <summary>
        /// AI服务，支持：deepseek、qwen
        /// </summary>
        public string provider { get; set; }

        /// <summary>
        /// 对话模型
        /// </summary>
        public string model { get; set; }
    }
}
