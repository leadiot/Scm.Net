using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Sync
{
    /// <summary>
    /// 同步日志
    /// </summary>
    [SugarTable("nas_log_file")]
    public class SyncLogFileDao : ScmDataDao
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required]
        public long user_id { get; set; }

        /// <summary>
        /// 终端ID
        /// </summary>
        [Required]
        public long terminal_id { get; set; }

        /// <summary>
        /// 驱动ID
        /// </summary>
        [Required]
        public long drive_id { get; set; }

        /// <summary>
        /// 目录ID
        /// </summary>
        [Required]
        public long dir_id { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [Required]
        public NasTypeEnums type { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Required]
        [StringLength(2048)]
        public string name { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [Required]
        [StringLength(2048)]
        public string path { get; set; }

        /// <summary>
        /// 文件摘要
        /// </summary>
        [StringLength(64)]
        public string hash { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Required]
        public NasOptEnums opt { get; set; }

        /// <summary>
        /// 同步方向
        /// </summary>
        [Required]
        public NasDirEnums dir { get; set; }

        /// <summary>
        /// 来源文件
        /// </summary>
        [StringLength(2048)]
        public string src { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long ver { get; set; }
    }
}