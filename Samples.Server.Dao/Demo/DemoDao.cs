using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Samples.Demo
{
    /// <summary>
    /// 演示对象DAO
    /// </summary>
    [SqlSugar.SugarTable("samples_demo")]
    public class DemoDao : ScmDataDao, ISystemDao, IDeleteDao
    {
        /// <summary>
        /// 选项
        /// </summary>
        public long option_id { get; set; }
        /// <summary>
        /// 系统编码（全局编码，用于系统时API交换）
        /// 格式：DEMO00000001
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codes { get; set; }
        /// <summary>
        /// 客户编码（客户自定义编码，具有业务含义）
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }
        /// <summary>
        /// 系统名称（简称，支持搜索）
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string names { get; set; }
        /// <summary>
        /// 客户名称（全称）
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string namec { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string remark { get; set; }

        /// <summary>
        /// 系统记录标识（不是必需）
        /// </summary>
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmRowSystemEnum row_system { get; set; } = ScmRowSystemEnum.No;

        /// <summary>
        /// 数据删除标识（不是必需）
        /// </summary>
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmRowDeleteEnum row_delete { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            row_delete = ScmRowDeleteEnum.No;

            // 新增时，自动生成系统编码
            codes = UidUtils.NextCodes("samples_demo");
            // 新增时，自动生成系统名称
            if (string.IsNullOrWhiteSpace(names))
            {
                names = namec;
            }
        }
    }
}
