using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Log
{
    /// <summary>
    /// 同步日志
    /// </summary>
    public class NasLogFileDto : ScmDataDto
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
        public long dir_id { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [Required]
        public NasTypeEnums type { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
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
        /// 更新时间
        /// </summary>
        public long modify_time { get; set; }

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
    }
}