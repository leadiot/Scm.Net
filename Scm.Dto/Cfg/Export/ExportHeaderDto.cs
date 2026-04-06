using Com.Scm.Dto;

namespace Com.Scm.Cfg.Export
{
    public class ExportHeaderDto : ScmDataDto
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string names { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string file { get; set; }

        public List<ExportDetailDto> details { get; set; }
    }
}
