using Com.Scm.Dao.User;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Table
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_sys_table_header")]
    public class SysTableHeaderDao : ScmUserDataDao
    {
        /// <summary>
        /// 系统代码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codes { get; set; }

        /// <summary>
        /// 视图编码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string names { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        [Required]
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmRowDeleteEnum row_delete { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            codes = UidUtils.NextCodes("scm_sys_table_header");
        }
    }
}