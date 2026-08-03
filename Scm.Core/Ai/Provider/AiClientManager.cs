using Com.Scm.Ai.Config;
using Com.Scm.Exceptions;
using System.Collections.Concurrent;

namespace Com.Scm.Ai.Provider
{
    /// <summary>
    /// AI客户端管理，按服务标识缓存客户端实例
    /// </summary>
    public class AiClientManager
    {
        private readonly AiConfig _config;
        private readonly ConcurrentDictionary<string, OpenAiCompatibleClient> _clients = new ConcurrentDictionary<string, OpenAiCompatibleClient>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public AiClientManager(AiConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// 解析AI服务标识，为空时使用默认对话服务
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string ResolveProvider(string provider)
        {
            return string.IsNullOrWhiteSpace(provider) ? _config.ChatProvider : provider;
        }

        /// <summary>
        /// 获取指定服务的客户端
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public OpenAiCompatibleClient GetClient(string provider)
        {
            var name = ResolveProvider(provider);
            var config = _config.GetProvider(name);
            if (config == null || !config.IsEnabled())
            {
                throw new BusinessException($"AI服务[{name}]未配置，请先配置 Ai:{name} 节点的 BaseUrl 与 ApiKey！");
            }

            return _clients.GetOrAdd(name, _ => new OpenAiCompatibleClient(config));
        }

        /// <summary>
        /// 获取对话模型名称
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string GetChatModel(string provider, string model)
        {
            if (!string.IsNullOrWhiteSpace(model))
            {
                return model;
            }

            var name = ResolveProvider(provider);
            return _config.GetProvider(name)?.ChatModel;
        }

        /// <summary>
        /// 获取向量化客户端
        /// </summary>
        /// <returns></returns>
        public OpenAiCompatibleClient GetEmbeddingClient()
        {
            return GetClient(_config.EmbeddingProvider);
        }

        /// <summary>
        /// 获取向量化模型名称
        /// </summary>
        /// <returns></returns>
        public string GetEmbeddingModel()
        {
            var config = _config.GetProvider(_config.EmbeddingProvider);
            if (string.IsNullOrWhiteSpace(config?.EmbeddingModel))
            {
                throw new BusinessException($"AI服务[{_config.EmbeddingProvider}]未配置向量化模型！");
            }

            return config.EmbeddingModel;
        }

        /// <summary>
        /// 批量文本向量化（自动分批）
        /// </summary>
        /// <param name="texts"></param>
        /// <returns></returns>
        public async Task<List<float[]>> EmbedAsync(IReadOnlyList<string> texts)
        {
            var client = GetEmbeddingClient();
            var model = GetEmbeddingModel();

            var batch = _config.Rag.EmbedBatch > 0 ? _config.Rag.EmbedBatch : 6;
            var result = new List<float[]>(texts.Count);

            for (var i = 0; i < texts.Count; i += batch)
            {
                var slice = texts.Skip(i).Take(batch).ToList();
                var vectors = await client.EmbedAsync(slice, model);
                result.AddRange(vectors);
            }

            return result;
        }
    }
}
