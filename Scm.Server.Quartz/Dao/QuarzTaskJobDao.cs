using Com.Scm.Dao;
using Com.Scm.Quartz.Enums;
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Quartz.Dao
{
    /// <summary>
    /// 任务
    /// </summary>
    [Description("任务表")]
    [SugarTable("scm_task_quarz")]
    public class QuarzTaskJobDao : ScmDataDao
    {
        /// <summary>
        /// 任务类型(1.DLL类型,2.API类型)
        /// </summary>
        [Description("任务类型(1.DLL类型,2.API类型)")]
        public TaskTypeEnum types { get; set; } = TaskTypeEnum.Dll;

        /// <summary>
        /// 任务名
        /// </summary>
        [Description("任务名")]
        [StringLength(128)]
        public string names { get; set; }
        /// <summary>
        /// 分组名
        /// </summary>
        [Description("分组名")]
        [StringLength(128)]
        public string group { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>
        [Description("间隔时间")]
        [StringLength(128)]
        public string cron { get; set; }

        /// <summary>
        /// 最近一次运行时间
        /// </summary>
        [Description("最近一次运行时间")]
        public DateTime? last_time { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        [Description("运行状态")]
        public JobHandleEnum handle { get; set; }

        /// <summary>
        /// 执行结果
        /// </summary>
        public JobResultEnum result { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        [Description("任务描述")]
        [StringLength(500)]
        public string remark { get; set; }

        #region Api类型专用参数
        /// <summary>
        /// 调用的API地址
        /// </summary>
        [Description("调用的API地址")]
        [StringLength(512)]
        public string api_uri { get; set; }

        /// <summary>
        /// API访问类型(API类型)
        /// </summary>
        [Description("API访问类型(API类型)")]
        [StringLength(100)]
        public string api_method { get; set; }
        /// <summary>
        /// 授权名(API类型)
        /// </summary>
        /// 
        [Description("API请求头")]
        [StringLength(1000)]
        public string api_headers { get; set; }
        /// <summary>
        /// API参数
        /// </summary>
        [Description("API参数")]
        [StringLength(1000)]
        public string api_parameter { get; set; }
        #endregion

        #region DLL类型专用参数
        /// <summary>
        /// DLL类型名
        /// </summary>
        [Description("DLL类型名")]
        [StringLength(30)]
        public string dll_uri { get; set; }

        /// <summary>
        /// Dll方法名
        /// </summary>
        /// 
        [Description("Dll方法名")]
        [StringLength(30)]
        public string dll_method { get; set; }

        /// <summary>
        /// DLL参数
        /// </summary>
        [Description("DLL参数")]
        [StringLength(1000)]
        public string dll_parameter { get; set; }
        #endregion
    }
}
