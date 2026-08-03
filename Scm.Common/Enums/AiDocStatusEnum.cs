using System.ComponentModel;

namespace Com.Scm.Enums
{
    /// <summary>
    /// AI知识库文档索引状态
    /// </summary>
    public enum AiDocStatusEnum
    {
        /// <summary>
        /// 待索引
        /// </summary>
        [Description("待索引")]
        Pending = 0,
        /// <summary>
        /// 索引中
        /// </summary>
        [Description("索引中")]
        Indexing = 1,
        /// <summary>
        /// 已索引
        /// </summary>
        [Description("已索引")]
        Indexed = 2,
        /// <summary>
        /// 索引失败
        /// </summary>
        [Description("索引失败")]
        Failed = 3
    }
}
