namespace Com.Scm.Ai.Dvo
{
    #region 协议模型
    /// <summary>
    /// 对话完成响应（OpenAI兼容协议）
    /// </summary>
    public class AiChatCompletion
    {
        public string id { get; set; }

        public string model { get; set; }

        public List<AiChatChoice> choices { get; set; }
    }
    #endregion
}
