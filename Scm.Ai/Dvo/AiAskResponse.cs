namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 知识库问答响应（RAG）
    /// </summary>
    public class AiAskResponse
    {
        /// <summary>
        /// 回答内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// AI服务
        /// </summary>
        public string provider { get; set; }

        /// <summary>
        /// 对话模型
        /// </summary>
        public string model { get; set; }

        /// <summary>
        /// 引用来源
        /// </summary>
        public List<AiAskSource> sources { get; set; } = new List<AiAskSource>();
    }
}
