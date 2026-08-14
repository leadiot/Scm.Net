namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 对话消息
    /// </summary>
    public class AiChatMessage
    {
        /// <summary>
        /// 角色：system、user、assistant
        /// </summary>
        public string role { get; set; } = "user";

        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
    }
}
