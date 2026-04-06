using Com.Scm.Dto;

namespace Com.Scm.Cfg.Export
{
    public class ExportDetailDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long export_id { get; set; }
        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        public string col { get; set; }
        /// <summary>
        /// 展示名称
        /// </summary>
        public string namec { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public string def { get; set; }
        /// <summary>
        /// 公式
        /// </summary>
        public string fun { get; set; }
    }
}
