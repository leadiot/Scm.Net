using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas.Res
{
    public class NasResFileDto : ScmDataDto
    {
        /// <summary>
        /// 驱动ID
        /// </summary>
        public long folder_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NasTypeEnums type { get; set; }

        /// <summary>
        /// 目录ID
        /// </summary>
        [Required]
        public long dir_id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(256)]
        public string name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [StringLength(2048)]
        public string path { get; set; }

        /// <summary>
        /// 文档摘要
        /// </summary>
        [StringLength(64)]
        public string hash { get; set; }

        /// <summary>
        /// 文档大小
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long modify_time { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        [Required]
        public long ver { get; set; }
    }
}
