using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Table
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_sys_table_detail")]
    public class SysTableDetailDao : ScmDataDao
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
        [SugarColumn(Length = 32, IsNullable = true)]
        public string label { get; set; }

        /// <summary>
        /// 数据字段
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
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
        [SugarColumn(Length = 16, IsNullable = true)]
        public string align { get; set; }

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool sortable { get; set; }
    }
}