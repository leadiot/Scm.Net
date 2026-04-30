using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Dto.Download
{
    /// <summary>
    /// 下载任务信息 DTO
    /// </summary>
    public class NasDownloadTaskDto : ScmDataDto
    {
        /// <summary>
        /// 原始下载链接
        /// </summary>
        [Required]
        [StringLength(2048)]
        public string Url { get; set; }

        /// <summary>
        /// 链接类型
        /// </summary>
        public NasDownloadLinkType LinkType { get; set; }

        /// <summary>
        /// 保存文件名
        /// </summary>
        [Required]
        [StringLength(256)]
        public string FileName { get; set; }

        /// <summary>
        /// 保存目录路径
        /// </summary>
        [Required]
        [StringLength(256)]
        public string SavePath { get; set; }

        /// <summary>
        /// 文件总大小（字节，-1 表示未知）
        /// </summary>
        public long TotalSize { get; set; }

        /// <summary>
        /// 已下载大小（字节）
        /// </summary>
        public long DownloadedSize { get; set; }

        /// <summary>
        /// 下载进度（0~100）
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// 当前速度（字节/秒）
        /// </summary>
        public long Speed { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public NasDownloadStatus Status { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public long FinishTime { get; set; }

        /// <summary>
        /// 并发线程数
        /// </summary>
        public int Threads { get; set; }
    }
}
