using Com.Scm.Dto;
using Com.Scm.Quartz.Enums;

namespace Com.Scm.Quartz.Dto
{
    public class QuartzTaskJobDto : ScmDto
    {
        /// <summary>
        /// 任务类型(1.DLL类型,2.API类型)
        /// </summary>
        public TaskTypeEnum types { get; set; } = TaskTypeEnum.Dll;

        /// <summary>
        /// 任务名
        /// </summary>
        public string names { get; set; }
        /// <summary>
        /// 分组名
        /// </summary>
        public string group { get; set; }
        /// <summary>
        /// 间隔时间
        /// </summary>
        public string cron { get; set; }

        /// <summary>
        /// 最近一次运行时间
        /// </summary>
        public DateTime? last_time { get; set; }

        /// <summary>
        /// 运行状态
        /// </summary>
        public JobHandleEnum handle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public JobResultEnum result { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string remark { get; set; }

        #region Api类型专用参数
        /// <summary>
        /// 调用的API地址
        /// </summary>
        public string api_uri { get; set; }

        /// <summary>
        /// API访问类型(API类型)
        /// </summary>
        public string api_method { get; set; }
        /// <summary>
        /// 授权名(API类型)
        /// </summary>
        public string api_auth_key { get; set; }
        /// <summary>
        /// 授权值(API类型)
        /// </summary>
        public string api_auth_value { get; set; }
        /// <summary>
        /// API参数
        /// </summary>
        public string api_parameter { get; set; }
        #endregion

        #region DLL类型专用参数
        /// <summary>
        /// DLL类型名
        /// </summary>
        public string dll_uri { get; set; }

        /// <summary>
        /// Dll方法名
        /// </summary>
        public string dll_method { get; set; }
        #endregion
    }
}
