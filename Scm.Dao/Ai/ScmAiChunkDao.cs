using Com.Scm.Dao;
using SqlSugar;

namespace Com.Scm.Ai
{
    /// <summary>
    /// AI知识库文档片段表
    /// </summary>
    [SugarTable("scm_ai_chunk")]
    public class ScmAiChunkDao : ScmDao
    {
        /// <summary>
        /// 文档ID
        /// </summary>
        public long doc_id { get; set; }

        /// <summary>
        /// 片段序号
        /// </summary>
        public int chunk_no { get; set; }

        /// <summary>
        /// 片段内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 向量维度
        /// </summary>
        public int dim { get; set; }

        /// <summary>
        /// 向量数据（JSON数组）
        /// </summary>
        public string vector { get; set; }
    }
}
