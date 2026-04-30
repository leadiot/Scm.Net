using Com.Scm.Nas.Dto.Download;

namespace Com.Scm.Nas.Download.Dvo
{
    /// <summary>
    /// 下载任务列表响应
    /// </summary>
    public class NasDownloadListResponse
    {
        /// <summary>
        /// 任务列表
        /// </summary>
        public List<NasDownloadTaskDto> Tasks { get; set; } = new();

        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }
    }
}
