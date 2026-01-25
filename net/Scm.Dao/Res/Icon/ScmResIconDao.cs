using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Icon
{
    /// <summary>
    /// 图标
    /// </summary>
    [SugarTable("scm_res_icon")]
    public class ScmResIconDao : ScmDataDao
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
        [SugarColumn(Length = 32)]
        public string key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string code { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string name { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string desc { get; set; }

        /// <summary>
        /// 笔画类型，both,line,fill
        /// </summary>
        [StringLength(8)]
        [SugarColumn(Length = 8, IsNullable = true)]
        public string type { get; set; }
    }
}
