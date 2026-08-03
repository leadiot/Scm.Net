using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ai
{
    /// <summary>
    /// AI知识库文档表
    /// </summary>
    [SugarTable("scm_ai_doc")]
    public class ScmAiDocDao : ScmDataDao
    {
        /// <summary>
        /// 文档名称
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string names { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        [StringLength(16)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string exts { get; set; }

        /// <summary>
        /// 文件相对路径
        /// </summary>
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string file_path { get; set; }

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
        /// 索引状态
        /// </summary>
        public AiDocStatusEnum status { get; set; } = AiDocStatusEnum.Pending;

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(512)]
        [SugarColumn(Length = 512, IsNullable = true)]
        public string remark { get; set; }
    }
}
