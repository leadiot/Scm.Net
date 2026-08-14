namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 智能问答响应
    /// </summary>
    public class AiChatResponse
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
    }
}
