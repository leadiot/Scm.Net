using Com.Scm.Ai.Config;
using Com.Scm.Ai.Dvo;
using Com.Scm.Ai.Provider;
using Com.Scm.Ai.Rag;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Filters;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Text;
using System.Text.Json;

namespace Com.Scm.Ai
{
    /// <summary>
    /// AI智能问答服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "ai")]
    public class ScmAiChatService : IApiService
    {
        private readonly AiConfig _aiConfig;
        private readonly AiClientManager _aiManager;
        private readonly ISqlSugarClient _sqlClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aiConfig"></param>
        /// <param name="aiManager"></param>
        /// <param name="sqlClient"></param>
        public ScmAiChatService(AiConfig aiConfig,
            AiClientManager aiManager,
            ISqlSugarClient sqlClient)
        {
            _aiConfig = aiConfig;
            _aiManager = aiManager;
            _sqlClient = sqlClient;

            AiDbSetup.EnsureTables(_sqlClient);
        }

        /// <summary>
        /// 智能问答
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AiChatResponse> ChatAsync(AiChatRequest request)
        {
            var messages = PrepareMessages(request);

            var provider = _aiManager.ResolveProvider(request.provider);
            var model = _aiManager.GetChatModel(request.provider, request.model);
            var client = _aiManager.GetClient(provider);

            var result = await client.ChatAsync(messages, model, request.temperature);

            return new AiChatResponse()
            {
                content = result.content,
                provider = provider,
                model = result.model ?? model
            };
        }

        /// <summary>
        /// 智能问答——流式输出（SSE）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, NoJsonResult]
        public async Task<IActionResult> ChatStreamAsync(AiChatRequest request)
        {
            var messages = PrepareMessages(request);

            var model = _aiManager.GetChatModel(request.provider, request.model);
            var client = _aiManager.GetClient(request.provider);

            var response = await client.ChatStreamAsync(messages, model, request.temperature);

            return new AiStreamResult(response);
        }

        /// <summary>
        /// 知识库问答（RAG）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AiAskResponse> AskAsync(AiAskRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.question))
            {
                throw new BusinessException("问题内容不能为空！");
            }

            // 1. 问题向量化
            var questionVector = await _aiManager.EmbedAsync(new List<string>() { request.question });
            if (questionVector.Count == 0 || questionVector[0] == null)
            {
                throw new BusinessException("问题向量化失败！");
            }

            // 2. 检索相似片段
            var topK = request.top_k > 0 ? request.top_k : _aiConfig.Rag.TopK;
            var sources = await SearchChunksAsync(questionVector[0], request.doc_id, topK);

            // 3. 组装提示词并调用大模型
            var provider = _aiManager.ResolveProvider(request.provider);
            var model = _aiManager.GetChatModel(request.provider, request.model);
            var client = _aiManager.GetClient(provider);

            var messages = BuildAskMessages(request.question, sources);
            var result = await client.ChatAsync(messages, model, 0.3f);

            return new AiAskResponse()
            {
                content = result.content,
                provider = provider,
                model = result.model ?? model,
                sources = sources
            };
        }

        /// <summary>
        /// 知识库片段检索
        /// </summary>
        /// <param name="questionVector"></param>
        /// <param name="docId"></param>
        /// <param name="topK"></param>
        /// <returns></returns>
        private async Task<List<AiAskSource>> SearchChunksAsync(float[] questionVector, long docId, int topK)
        {
            var chunkList = await _sqlClient.Queryable<ScmAiChunkDao>()
                .WhereIF(docId > 0, m => m.doc_id == docId)
                .ToListAsync();
            if (chunkList.Count == 0)
            {
                throw new BusinessException("知识库暂无已索引的文档，请先上传文档！");
            }

            var docDict = (await _sqlClient.Queryable<ScmAiDocDao>()
                    .Where(m => m.status == AiDocStatusEnum.Indexed)
                    .ToListAsync())
                .ToDictionary(a => a.id, a => a.names);

            // 余弦相似度计算与排序
            var scored = new List<AiAskSource>();
            foreach (var chunk in chunkList)
            {
                if (string.IsNullOrEmpty(chunk.vector))
                {
                    continue;
                }

                var vector = JsonSerializer.Deserialize<float[]>(chunk.vector);
                var score = AiVectorUtils.Cosine(questionVector, vector);
                if (score < _aiConfig.Rag.MinScore)
                {
                    continue;
                }

                scored.Add(new AiAskSource()
                {
                    doc_id = chunk.doc_id,
                    doc_name = docDict.TryGetValue(chunk.doc_id, out var name) ? name : "",
                    chunk_no = chunk.chunk_no,
                    score = score,
                    content = chunk.content
                });
            }

            return scored.OrderByDescending(a => a.score).Take(topK).ToList();
        }

        /// <summary>
        /// 组装知识库问答消息
        /// </summary>
        private List<AiChatMessage> BuildAskMessages(string question, List<AiAskSource> sources)
        {
            var builder = new StringBuilder();
            builder.AppendLine("你是一个企业知识库助手，请根据下面提供的参考资料回答问题。");
            builder.AppendLine("要求：");
            builder.AppendLine("1、仅根据参考资料作答，资料中没有的信息请明确说明\"资料中未提及\"；");
            builder.AppendLine("2、回答尽量简洁准确，并在句末标注引用的片段编号，如[1]。");
            builder.AppendLine();
            builder.AppendLine("参考资料：");

            var index = 1;
            foreach (var source in sources)
            {
                builder.AppendLine($"[{index++}]（文档：{source.doc_name}，片段：{source.chunk_no}）");
                builder.AppendLine(source.content);
                builder.AppendLine();
            }

            builder.AppendLine("问题：" + question);

            return new List<AiChatMessage>()
            {
                new AiChatMessage() { role = "user", content = builder.ToString() }
            };
        }

        /// <summary>
        /// 组装对话消息
        /// </summary>
        private List<AiChatMessage> PrepareMessages(AiChatRequest request)
        {
            if (request == null || request.messages == null || request.messages.Count == 0)
            {
                throw new BusinessException("对话消息不能为空！");
            }

            var messages = new List<AiChatMessage>();
            if (!string.IsNullOrWhiteSpace(request.system))
            {
                messages.Add(new AiChatMessage() { role = "system", content = request.system });
            }

            messages.AddRange(request.messages);
            return messages;
        }
    }
}
