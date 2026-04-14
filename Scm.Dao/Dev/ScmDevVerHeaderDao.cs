using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev
{
    /// <summary>
    /// 升级日志信息
    /// </summary>
    [SugarTable("scm_sys_ver_header")]
    public class ScmDevVerHeaderDao : ScmDataDao
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
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string ver { get; set; }

        /// <summary>
        /// 主版本号
        /// </summary>
        public int major { get; set; }

        /// <summary>
        /// 子版本号
        /// </summary>
        public int minor { get; set; }

        /// <summary>
        /// 修订版本号
        /// </summary>
        public int patch { get; set; }

        /// <summary>
        /// 构建版本号，默认自增
        /// </summary>
        public int build { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        [Required]
        [StringLength(10)]
        [SugarColumn(Length = 10)]
        public string release_date { get; set; }

        /// <summary>
        /// 发行版本
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string release_code { get; set; }

        /// <summary>
        /// 最小版本
        /// </summary>
        [StringLength(16)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string ver_min { get; set; }

        /// <summary>
        /// 最大版本
        /// </summary>
        [StringLength(16)]
        [SugarColumn(Length = 16, IsNullable = true)]
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
        /// 修选版本
        /// </summary>
        public bool rc { get; set; }

        /// <summary>
        /// 正式版本
        /// </summary>
        public bool ga { get; set; }

        /// <summary>
        /// 强制更新
        /// </summary>
        public bool forced { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public bool current { get; set; }

        /// <summary>
        /// 升级说明(简介)
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string remark { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string url { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int size { get; set; }
    }
}