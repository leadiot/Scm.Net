namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 对话选项
    /// </summary>
    public class AiChatChoice
    {
        public int index { get; set; }

        public AiChatMessage message { get; set; }

        /// <summary>
        /// 流式返回的增量内容
        /// </summary>
        public AiChatMessage delta { get; set; }

        public string finish_reason { get; set; }
    }
}
