using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Ver
{
    /// <summary>
    /// 升级日志信息
    /// </summary>
    [SugarTable("scm_sys_ver_header")]
    public class ScmSysVerHeaderDao : ScmDataDao
    {
        /// <summary>
        /// 应用
        /// </summary>
        [Required]
        public ScmClientTypeEnum client { get; set; }

        /// <summary>
        /// 系统
        /// </summary>
        public long app_id { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        [StringLength(16)]
        public string ver { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// 构建版本
        /// </summary>
        [StringLength(16)]
        public string build { get; set; }

        /// <summary>
        /// 最小版本
        /// </summary>
        [StringLength(16)]
        public string ver_min { get; set; }

        /// <summary>
        /// 最大版本
        /// </summary>
        [StringLength(16)]
        public string ver_max { get; set; }

        /// <summary>
        /// 是否内测
        /// </summary>
        public bool alpha { get; set; }

        /// <summary>
        /// 是否公测
        /// </summary>
        public bool beta { get; set; }

        /// <summary>
        /// 强制更新
        /// </summary>
        public bool forced { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public bool current { get; set; }

        /// <summary>
        /// 应用路径（首页、下载等）
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 升级说明(简介)
        /// </summary>
        [StringLength(1024)]
        public string remark { get; set; }
    }
}