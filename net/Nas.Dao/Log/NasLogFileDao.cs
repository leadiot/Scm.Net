using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Log
{
    /// <summary>
    /// 同步日志
    /// </summary>
    [SugarTable("nas_log_file")]
    public class NasLogFileDao : ScmUserDataDao
    {
        /// <summary>
        /// 终端ID
        /// </summary>
        [Required]
        public long terminal_id { get; set; }

        /// <summary>
        /// 驱动ID
        /// </summary>
        [Required]
        public long folder_id { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>
        [Required]
        public long res_id { get; set; }

        /// <summary>
        /// 目录ID
        /// </summary>
        [Required]
        public long dir_id { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [Required]
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmFileTypeEnum type { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string name { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [Required]
        [StringLength(1024)]
        [SugarColumn(Length = 1024)]
        public string path { get; set; }

        /// <summary>
        /// 文件摘要
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string hash { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        [Required]
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public NasOptEnums opt { get; set; }

        /// <summary>
        /// 同步方向
        /// </summary>
        [Required]
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public NasDirEnums dir { get; set; }

        /// <summary>
        /// 来源文件
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string src { get; set; }

        /// <summary>
        /// 文件修改时间
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long ver { get; set; }
    }
}