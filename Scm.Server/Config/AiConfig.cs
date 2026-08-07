using Com.Scm.Config;

namespace Com.Scm.Ai.Config
{
    /// <summary>
    /// AI大模型配置
    /// </summary>
    public class AiConfig
    {
        /// <summary>
        /// 配置节点名称
        /// </summary>
        public const string NAME = "Ai";

        /// <summary>
        /// DeepSeek服务标识
        /// </summary>
        public const string PROVIDER_DEEPSEEK = "deepseek";

        /// <summary>
        /// 通义千问服务标识
        /// </summary>
        public const string PROVIDER_QWEN = "qwen";

        /// <summary>
        /// 默认对话服务，取Providers列表中某项的Code
        /// </summary>
        public string ChatProvider { get; set; } = PROVIDER_DEEPSEEK;

        /// <summary>
        /// 默认向量化服务，取Providers列表中某项的Code（DeepSeek暂未提供向量化接口，建议qwen）
        /// </summary>
        public string EmbeddingProvider { get; set; } = PROVIDER_QWEN;

        /// <summary>
        /// AI服务列表配置
        /// </summary>
        public List<AiProviderConfig> Providers { get; set; }

        /// <summary>
        /// RAG知识库配置
        /// </summary>
        public AiRagConfig Rag { get; set; } = new AiRagConfig();

        /// <summary>
        /// 预处理配置
        /// </summary>
        /// <param name="envConfig"></param>
        public void Prepare(EnvConfig envConfig)
        {
            Rag.Prepare(envConfig);

            if (Providers == null)
            {
                Providers = new List<AiProviderConfig>();
            }
            if (Providers.Count < 1)
            {
                Providers.Add(new AiProviderConfig()
                {
                    Code = PROVIDER_DEEPSEEK,
                    Name = "DeepSeek",
                    BaseUrl = "https://api.deepseek.com/v1",
                    ChatModel = "deepseek-chat"
                });
                Providers.Add(new AiProviderConfig()
                {
                    Code = PROVIDER_QWEN,
                    Name = "通义千问",
                    BaseUrl = "https://dashscope.aliyuncs.com/compatible-mode/v1",
                    ChatModel = "qwen-plus",
                    EmbeddingModel = "text-embedding-v3"
                });
            }
        }

        /// <summary>
        /// 按服务标识获取服务配置，未找到时返回null
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public AiProviderConfig GetProvider(string provider)
        {
            return Providers?.FirstOrDefault(p => p.Enabled && string.Equals(p.Code, provider, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// AI服务配置（OpenAI兼容协议）
    /// </summary>
    public class AiProviderConfig
    {
        /// <summary>
        /// 服务标识，如：deepseek、qwen
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 服务名称，如：DeepSeek、通义千问
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 接口秘钥
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// 对话模型
        /// </summary>
        public string ChatModel { get; set; }

        /// <summary>
        /// 向量化模型
        /// </summary>
        public string EmbeddingModel { get; set; }

        /// <summary>
        /// 请求超时时间（秒）
        /// </summary>
        public int Timeout { get; set; } = 300;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(BaseUrl) && !string.IsNullOrWhiteSpace(ApiKey);
        }
    }

    /// <summary>
    /// RAG知识库配置
    /// </summary>
    public class AiRagConfig
    {
        /// <summary>
        /// 知识库文档目录，默认相对于环境的dataDir目录
        /// </summary>
        public string DocDir { get; set; } = "ai/docs";

        /// <summary>
        /// 分块大小（字符数）
        /// </summary>
        public int ChunkSize { get; set; } = 500;

        /// <summary>
        /// 分块重叠（字符数）
        /// </summary>
        public int ChunkOverlap { get; set; } = 50;

        /// <summary>
        /// 检索返回的片段数量
        /// </summary>
        public int TopK { get; set; } = 4;

        /// <summary>
        /// 最低相似度阈值（余弦相似度，0~1）
        /// </summary>
        public double MinScore { get; set; } = 0.2;

        /// <summary>
        /// 向量化批次大小
        /// </summary>
        public int EmbedBatch { get; set; } = 6;

        /// <summary>
        /// 文档目录物理路径
        /// </summary>
        public string DocPath { get; set; }

        /// <summary>
        /// 预处理配置
        /// </summary>
        /// <param name="envConfig"></param>
        public void Prepare(EnvConfig envConfig)
        {
            DocPath = envConfig.GetDataPath(DocDir);
        }
    }
}
