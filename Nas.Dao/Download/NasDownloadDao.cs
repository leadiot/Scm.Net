using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Download
{
    [SugarTable("nas_download")]
    public class NasDownloadDao : ScmDataDao
    {
        /// <summary>
        /// 原始下载链接
        /// </summary>
        [Required]
        [StringLength(2048)]
        [SugarColumn(Length = 2048)]
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
        [SugarColumn(Length = 256)]
        public string FileName { get; set; }

        /// <summary>
        /// 保存目录路径
        /// </summary>
        [Required]
        [StringLength(512)]
        [SugarColumn(Length = 512)]
        public string SavePath { get; set; }

        /// <summary>
        /// 并发线程数
        /// </summary>
        public int Threads { get; set; } = 4;

        /// <summary>
        /// FTP 用户名（可选）
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string FtpUser { get; set; }

        /// <summary>
        /// FTP 密码（可选）
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string FtpPassword { get; set; }

        /// <summary>
        /// 文件总大小（字节，-1 表示未知）
        /// </summary>
        public long TotalSize { get; set; } = -1;

        /// <summary>
        /// 已下载大小（字节）
        /// </summary>
        public long DownloadedSize { get; set; }

        /// <summary>
        /// 下载进度（0~100）
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public NasDownloadStatus Status { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [StringLength(512)]
        [SugarColumn(Length = 512, IsNullable = true)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public long FinishTime { get; set; }
    }
}
