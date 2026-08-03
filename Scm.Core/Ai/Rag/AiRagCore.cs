using SqlSugar;

namespace Com.Scm.Ai.Rag
{
    /// <summary>
    /// AI知识库建表辅助
    /// </summary>
    public static class AiDbSetup
    {
        private static readonly object _lock = new object();
        private static bool _inited;

        /// <summary>
        /// 确保知识库相关表已创建
        /// </summary>
        /// <param name="client"></param>
        public static void EnsureTables(ISqlSugarClient client)
        {
            if (_inited)
            {
                return;
            }

            lock (_lock)
            {
                if (_inited)
                {
                    return;
                }

                client.CodeFirst.InitTables(typeof(ScmAiDocDao), typeof(ScmAiChunkDao));
                _inited = true;
            }
        }
    }

    /// <summary>
    /// 知识库文档文本提取，支持纯文本类文档
    /// </summary>
    public static class AiDocReader
    {
        /// <summary>
        /// 支持的文件扩展名
        /// </summary>
        public static readonly string[] AcceptExts = { ".txt", ".md", ".csv", ".json", ".log" };

        /// <summary>
        /// 是否为支持的文件类型
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static bool IsAccept(string ext)
        {
            if (string.IsNullOrEmpty(ext))
            {
                return false;
            }

            return AcceptExts.Contains(ext.ToLower());
        }

        /// <summary>
        /// 读取文档文本内容
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadText(string file)
        {
            return File.ReadAllText(file, System.Text.Encoding.UTF8);
        }
    }

    /// <summary>
    /// 文本分块器
    /// </summary>
    public static class AiTextSplitter
    {
        /// <summary>
        /// 按段落优先、固定长度切分文本
        /// </summary>
        /// <param name="text">原始文本</param>
        /// <param name="chunkSize">分块大小（字符数）</param>
        /// <param name="overlap">分块重叠（字符数）</param>
        /// <returns></returns>
        public static List<string> Split(string text, int chunkSize, int overlap)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(text))
            {
                return result;
            }

            if (chunkSize <= 0)
            {
                chunkSize = 500;
            }
            if (overlap < 0 || overlap >= chunkSize)
            {
                overlap = 0;
            }

            // 先按段落拆分，再将段落合并到指定大小的块中
            var paragraphs = text.Replace("\r\n", "\n")
                .Split('\n')
                .Select(a => a.Trim())
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .ToList();

            var buffer = new System.Text.StringBuilder();
            foreach (var para in paragraphs)
            {
                if (buffer.Length > 0 && buffer.Length + para.Length + 1 > chunkSize)
                {
                    result.Add(buffer.ToString());
                    buffer = CarryTail(buffer.ToString(), overlap);
                }

                if (para.Length > chunkSize)
                {
                    // 超长段落强制按长度切分
                    if (buffer.Length > 0)
                    {
                        result.Add(buffer.ToString());
                        buffer.Clear();
                    }

                    for (var i = 0; i < para.Length; i += chunkSize - overlap)
                    {
                        var len = Math.Min(chunkSize, para.Length - i);
                        result.Add(para.Substring(i, len));
                        if (i + len >= para.Length)
                        {
                            break;
                        }
                    }
                    continue;
                }

                if (buffer.Length > 0)
                {
                    buffer.Append('\n');
                }
                buffer.Append(para);
            }

            if (buffer.Length > 0)
            {
                result.Add(buffer.ToString());
            }

            return result;
        }

        /// <summary>
        /// 保留上一块尾部内容作为重叠
        /// </summary>
        private static System.Text.StringBuilder CarryTail(string prev, int overlap)
        {
            var buffer = new System.Text.StringBuilder();
            if (overlap > 0 && prev.Length > 0)
            {
                var len = Math.Min(overlap, prev.Length);
                buffer.Append(prev.Substring(prev.Length - len));
            }

            return buffer;
        }
    }

    /// <summary>
    /// 向量计算工具
    /// </summary>
    public static class AiVectorUtils
    {
        /// <summary>
        /// 余弦相似度
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Cosine(float[] a, float[] b)
        {
            if (a == null || b == null || a.Length != b.Length || a.Length == 0)
            {
                return 0;
            }

            double dot = 0, normA = 0, normB = 0;
            for (var i = 0; i < a.Length; i++)
            {
                dot += a[i] * (double)b[i];
                normA += a[i] * (double)a[i];
                normB += b[i] * (double)b[i];
            }

            if (normA == 0 || normB == 0)
            {
                return 0;
            }

            return dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }
    }
}
