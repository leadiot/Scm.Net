using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Download.Dvo
{
    /// <summary>
    /// 添加下载任务请求
    /// </summary>
    public class NasDownloadAddRequest
    {
        /// <summary>
        /// 下载链接（支持 http/https/ftp/file/nas:/ 协议）
        /// </summary>
        [Required]
        [StringLength(2048)]
        public string Url { get; set; }

        /// <summary>
        /// 保存到 NAS 的目标目录路径（留空则存入 /Downloads）
        /// </summary>
        [StringLength(256)]
        public string SavePath { get; set; }

        /// <summary>
        /// 保存文件名（留空则从 URL 自动推断）
        /// </summary>
        [StringLength(256)]
        public string SaveName { get; set; }

        /// <summary>
        /// 并发分片数（仅 HTTP 支持，默认 4，最大 16）
        /// </summary>
        public int Threads { get; set; } = 4;

        /// <summary>
        /// 下载完成后自动解压（仅压缩包有效）
        /// </summary>
        public bool AutoExtract { get; set; } = false;

        /// <summary>
        /// FTP 用户名（Ftp 链接时使用）
        /// </summary>
        public string FtpUser { get; set; }

        /// <summary>
        /// FTP 密码（Ftp 链接时使用）
        /// </summary>
        public string FtpPassword { get; set; }
    }
}
