namespace Com.Scm.Ai.Dvo
{
    /// <summary>
    /// 向量化响应（OpenAI兼容协议）
    /// </summary>
    public class AiEmbeddingResult
    {
        public string model { get; set; }

        public List<AiEmbeddingData> data { get; set; }
    }
}
