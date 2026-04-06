using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Table
{
    /// <summary>
    /// 
    /// </summary>
    public class SysTableDetailDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 显示标题
        /// </summary>
        [StringLength(32)]
        public string label { get; set; }

        /// <summary>
        /// 数据字段
        /// </summary>
        [StringLength(32)]
        public string prop { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool hide { get; set; }

        /// <summary>
        /// 列宽
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 对齐
        /// </summary>
        [StringLength(16)]
        public string align { get; set; }

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool sortable { get; set; }
    }
}