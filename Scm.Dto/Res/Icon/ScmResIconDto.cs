using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Icon
{
    public class ScmResIconDto : ScmDataDto
    {
        /// <summary>
        /// 图标集合，vue,sc,ms
        /// </summary>
        public long set_id { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public long cat_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        [StringLength(32)]
        public string key { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        [StringLength(32)]
        public string name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [StringLength(64)]
        public string desc { get; set; }

        /// <summary>
        /// 笔画类型，both,line,fill
        /// </summary>
        public string type { get; set; }
    }
}
