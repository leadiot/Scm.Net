using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Lang
{
    [SugarTable("scm_sys_lang")]
    public class LangDao : ScmDao
    {
        /// <summary>
        /// 语言编码
        /// </summary>
        [Required]
        [StringLength(8)]
        [SugarColumn(Length = 8)]
        public string code { get; set; }
        /// <summary>
        /// 语言名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string text { get; set; }
        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public ScmRowStatusEnum row_status { get; set; }
    }
}
