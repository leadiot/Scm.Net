using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Dto.Download
{
    /// <summary>
    /// 下载任务信息 DTO
    /// </summary>
    public class NasDownloadDto : ScmDataDto
    {
        /// <summary>
        /// 原始下载链接
        /// </summary>
        [Required]
        [StringLength(2048)]
        public string url { get; set; }

        /// <summary>
        /// 链接类型
        /// </summary>
        public NasDownloadLinkType link_type { get; set; }

        /// <summary>
        /// 保存文件名
        /// </summary>
        [Required]
        [StringLength(256)]
        public string file_name { get; set; }

        /// <summary>
        /// 保存目录路径
        /// </summary>
        [Required]
        [StringLength(256)]
        public string file_path { get; set; }

        /// <summary>
        /// 文件总大小（字节，-1 表示未知）
        /// </summary>
        public long total_size { get; set; }

        /// <summary>
        /// 已下载大小（字节）
        /// </summary>
        public long downloaded_size { get; set; }

        /// <summary>
        /// 下载进度（0~100）
        /// </summary>
        public double progress { get; set; }

        /// <summary>
        /// 当前速度（字节/秒）
        /// </summary>
        public long speed { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public ScmHandleEnum handle { get; set; }

        public ScmResultEnum result { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public long finish_time { get; set; }

        /// <summary>
        /// 并发线程数
        /// </summary>
        public int threads { get; set; }
    }
}
