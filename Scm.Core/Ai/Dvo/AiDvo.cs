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

    /// <summary>
    /// 向量化响应（OpenAI兼容协议）
    /// </summary>
    public class AiEmbeddingResult
    {
        public string model { get; set; }

        public List<AiEmbeddingData> data { get; set; }
    }

    /// <summary>
    /// 向量化数据
    /// </summary>
    public class AiEmbeddingData
    {
        public int index { get; set; }

        public float[] embedding { get; set; }
    }
    #endregion

    #region 请求响应
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

    /// <summary>
    /// 智能问答响应
    /// </summary>
    public class AiChatResponse
    {
        /// <summary>
        /// 回答内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// AI服务
        /// </summary>
        public string provider { get; set; }

        /// <summary>
        /// 对话模型
        /// </summary>
        public string model { get; set; }
    }

    /// <summary>
    /// 对话结果
    /// </summary>
    public class AiChatResult
    {
        /// <summary>
        /// 回答内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 对话模型
        /// </summary>
        public string model { get; set; }
    }

    /// <summary>
    /// 知识库问答请求（RAG）
    /// </summary>
    public class AiAskRequest
    {
        /// <summary>
        /// 问题内容
        /// </summary>
        public string question { get; set; }

        /// <summary>
        /// 知识库文档ID，0表示全部文档
        /// </summary>
        public long doc_id { get; set; }

        /// <summary>
        /// 检索返回的片段数量，0表示使用配置默认值
        /// </summary>
        public int top_k { get; set; }

        /// <summary>
        /// AI服务，支持：deepseek、qwen
        /// </summary>
        public string provider { get; set; }

        /// <summary>
        /// 对话模型
        /// </summary>
        public string model { get; set; }
    }

    /// <summary>
    /// 知识库问答响应（RAG）
    /// </summary>
    public class AiAskResponse
    {
        /// <summary>
        /// 回答内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// AI服务
        /// </summary>
        public string provider { get; set; }

        /// <summary>
        /// 对话模型
        /// </summary>
        public string model { get; set; }

        /// <summary>
        /// 引用来源
        /// </summary>
        public List<AiAskSource> sources { get; set; } = new List<AiAskSource>();
    }

    /// <summary>
    /// 知识库引用来源
    /// </summary>
    public class AiAskSource
    {
        /// <summary>
        /// 文档ID
        /// </summary>
        public long doc_id { get; set; }

        /// <summary>
        /// 文档名称
        /// </summary>
        public string doc_name { get; set; }

        /// <summary>
        /// 片段序号
        /// </summary>
        public int chunk_no { get; set; }

        /// <summary>
        /// 相似度得分
        /// </summary>
        public double score { get; set; }

        /// <summary>
        /// 片段内容
        /// </summary>
        public string content { get; set; }
    }

    /// <summary>
    /// 知识库文档
    /// </summary>
    public class AiDocDto
    {
        /// <summary>
        /// 唯一编号
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 文档名称
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string exts { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long file_size { get; set; }

        /// <summary>
        /// 字符数量
        /// </summary>
        public long char_count { get; set; }

        /// <summary>
        /// 片段数量
        /// </summary>
        public int chunk_count { get; set; }

        /// <summary>
        /// 索引状态：0待索引、1索引中、2已索引、3索引失败
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long create_time { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>
        public long create_user { get; set; }
    }

    /// <summary>
    /// 知识库文档上传请求
    /// </summary>
    public class AiDocUploadRequest
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
    #endregion
}
