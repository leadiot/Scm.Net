namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 智能问答请求
    /// </summary>
    public class AiChatRequest
    {
        /// <summary>
        /// AI服务，支持：deepseek、qwen，默认使用配置中的ChatProvider
        /// </summary>
        public string provider { get; set; }

        /// <summary>
        /// 对话模型，默认使用配置中的模型
        /// </summary>
        public string model { get; set; }

        /// <summary>
        /// 系统提示词
        /// </summary>
        public string system { get; set; }

        /// <summary>
        /// 对话消息列表
        /// </summary>
        public List<AiChatMessage> messages { get; set; }

        /// <summary>
        /// 温度参数（0~2），越大回答越发散
        /// </summary>
        public float temperature { get; set; } = 0.7f;
    }

}
