using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_sys_task")]
    public class TaskDao : ScmDataDao, IDeleteDao
    {
        /// <summary>
        /// 导出类型
        /// </summary>
        public TaskTypesEnum types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codes { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string names { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string clazz { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        [Required]
        [StringLength(1024)]
        [SugarColumn(Length = 1024)]
        public string json { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string file { get; set; }

        /// <summary>
        /// 作业状态
        /// </summary>
        public ScmHandleEnum handle { get; set; }
        /// <summary>
        /// 作业结果
        /// </summary>
        public ScmResultEnum result { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string message { get; set; }

        /// <summary>
        /// 预计执行时间（起始）
        /// </summary>
        public long need_time_f { get; set; }
        /// <summary>
        /// 预计执行时间（结束）
        /// </summary>
        public long need_time_t { get; set; }
        /// <summary>
        /// 实际执行时间（实际）
        /// </summary>
        public long exec_time_f { get; set; }
        /// <summary>
        /// 实际执行时间（结束）
        /// </summary>
        public long exec_time_t { get; set; }

        /// <summary>
        /// 
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

            row_delete = ScmRowDeleteEnum.No;
            codes = UidUtils.NextCodes("scm_sys_task");
        }
    }
}