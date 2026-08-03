using Com.Scm;
using Com.Scm.Ai.Config;
using Com.Scm.Ai.Dvo;
using Com.Scm.Ai.Provider;
using Com.Scm.Ai.Rag;
using Com.Scm.Config;
using Com.Scm.Dto;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Text.Json;

namespace Com.Scm.Ai
{
    /// <summary>
    /// AI知识库文档服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "Ai")]
    public class ScmAiDocService : IApiService
    {
        private readonly EnvConfig _envConfig;
        private readonly AiConfig _aiConfig;
        private readonly AiClientManager _aiManager;
        private readonly ISqlSugarClient _sqlClient;
        private readonly IJwtTokenHolder _jwtHolder;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="envConfig"></param>
        /// <param name="aiConfig"></param>
        /// <param name="aiManager"></param>
        /// <param name="sqlClient"></param>
        /// <param name="jwtHolder"></param>
        /// <param name="httpContextAccessor"></param>
        public ScmAiDocService(EnvConfig envConfig,
            AiConfig aiConfig,
            AiClientManager aiManager,
            ISqlSugarClient sqlClient,
            IJwtTokenHolder jwtHolder,
            IHttpContextAccessor httpContextAccessor)
        {
            _envConfig = envConfig;
            _aiConfig = aiConfig;
            _aiManager = aiManager;
            _sqlClient = sqlClient;
            _jwtHolder = jwtHolder;
            _httpContextAccessor = httpContextAccessor;

            AiDbSetup.EnsureTables(_sqlClient);
        }

        /// <summary>
        /// 查询知识库文档——分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ScmPageResultDto<AiDocDto>> GetPagesAsync(SearchRequest param)
        {
            var query = await _sqlClient.Queryable<ScmAiDocDao>()
                .WhereIF(!string.IsNullOrEmpty(param.key), m => m.names.Contains(param.key))
                .WhereIF(param.status >= 0, m => m.status == (AiDocStatusEnum)param.status)
                .OrderBy(m => m.id, OrderByType.Desc)
                .Select<AiDocDto>()
                .ToPageAsyncV2(param.page, param.limit);

            return query;
        }

        /// <summary>
        /// 上传并索引知识库文档
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AiDocDto> UploadAsync([FromForm] AiDocUploadRequest request)
        {
            var files = _httpContextAccessor.HttpContext?.Request.Form.Files;
            if (files == null || files.Count == 0)
            {
                throw new BusinessException("上传文件不能为空！");
            }

            var file = files[0];
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!AiDocReader.IsAccept(ext))
            {
                throw new BusinessException($"不支持的文件类型：{ext}，仅支持：{string.Join("、", AiDocReader.AcceptExts)}");
            }

            var userId = _jwtHolder.GetToken().user_id;

            // 保存文件
            FileUtils.CreateDir(_aiConfig.Rag.DocPath);
            var fileName = UidUtils.NextId() + ext;
            var dstFile = Path.Combine(_aiConfig.Rag.DocPath, fileName);
            using (var stream = System.IO.File.OpenWrite(dstFile))
            {
                await file.CopyToAsync(stream);
            }

            // 保存文档记录
            var dao = new ScmAiDocDao()
            {
                names = file.FileName,
                exts = ext,
                file_path = Path.Combine(_aiConfig.Rag.DocDir, fileName).Replace('\\', '/'),
                file_size = file.Length,
                status = AiDocStatusEnum.Pending,
                remark = request?.remark
            };
            dao.PrepareCreate(userId);
            await _sqlClient.Insertable(dao).ExecuteCommandAsync();

            // 执行索引
            await IndexDocAsync(dao, userId);

            return await GetAsync(dao.id);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<AiDocDto> GetAsync(long id)
        {
            return await _sqlClient.Queryable<ScmAiDocDao>()
                .Select<AiDocDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 重建文档索引
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RebuildAsync(long id)
        {
            var dao = await _sqlClient.Queryable<ScmAiDocDao>().FirstAsync(m => m.id == id);
            if (dao == null)
            {
                throw new BusinessException("文档不存在！");
            }

            var userId = _jwtHolder.GetToken().user_id;
            await IndexDocAsync(dao, userId);
            return true;
        }

        /// <summary>
        /// 删除知识库文档，支持多个
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> DeleteAsync(string ids)
        {
            var idList = ids.ToListLong();
            if (idList.Count == 0)
            {
                return 0;
            }

            // 删除文件与片段
            var docList = await _sqlClient.Queryable<ScmAiDocDao>().Where(m => idList.Contains(m.id)).ToListAsync();
            foreach (var doc in docList)
            {
                var file = _envConfig.GetDataPath(doc.file_path);
                FileUtils.DeleteDoc(file);
            }

            await _sqlClient.Deleteable<ScmAiChunkDao>().Where(m => idList.Contains(m.doc_id)).ExecuteCommandAsync();

            return await _sqlClient.Deleteable<ScmAiDocDao>().Where(m => idList.Contains(m.id)).ExecuteCommandAsync();
        }

        /// <summary>
        /// 解析文档并建立向量索引
        /// </summary>
        private async Task IndexDocAsync(ScmAiDocDao dao, long userId)
        {
            try
            {
                dao.status = AiDocStatusEnum.Indexing;
                dao.PrepareUpdate(userId);
                await _sqlClient.Updateable(dao).ExecuteCommandAsync();

                // 清理旧片段
                await _sqlClient.Deleteable<ScmAiChunkDao>().Where(m => m.doc_id == dao.id).ExecuteCommandAsync();

                // 提取文本并分块
                var file = _envConfig.GetDataPath(dao.file_path);
                var text = AiDocReader.ReadText(file);
                var chunks = AiTextSplitter.Split(text, _aiConfig.Rag.ChunkSize, _aiConfig.Rag.ChunkOverlap);
                if (chunks.Count == 0)
                {
                    throw new BusinessException("文档内容为空，无法建立索引！");
                }

                // 向量化
                var vectors = await _aiManager.EmbedAsync(chunks);

                // 保存片段
                var chunkList = new List<ScmAiChunkDao>();
                for (var i = 0; i < chunks.Count; i++)
                {
                    chunkList.Add(new ScmAiChunkDao()
                    {
                        id = UidUtils.NextId(),
                        doc_id = dao.id,
                        chunk_no = i,
                        content = chunks[i],
                        dim = vectors[i]?.Length ?? 0,
                        vector = JsonSerializer.Serialize(vectors[i])
                    });
                }
                await _sqlClient.Insertable(chunkList).ExecuteCommandAsync();

                // 更新文档状态
                dao.status = AiDocStatusEnum.Indexed;
                dao.char_count = text.Length;
                dao.chunk_count = chunks.Count;
                dao.remark = string.Empty;
                dao.PrepareUpdate(userId);
                await _sqlClient.Updateable(dao).ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex);

                dao.status = AiDocStatusEnum.Failed;
                dao.remark = ex.Message;
                if (dao.remark.Length > 500)
                {
                    dao.remark = dao.remark.Substring(0, 500);
                }
                dao.PrepareUpdate(userId);
                await _sqlClient.Updateable(dao).ExecuteCommandAsync();

                throw;
            }
        }
    }
}
