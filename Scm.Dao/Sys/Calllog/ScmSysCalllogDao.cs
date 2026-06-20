using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Calllog
{
    [SugarTable("scm_sys_calllog")]
    public class ScmSysCalllogDao : ScmUserDataDao
    {
        /// <summary>
        /// 号码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string number { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string name { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public long date { get; set; }

        /// <summary>
        /// 呼叫类型
        /// </summary>
        public ScmCallTypeEnum type { get; set; }

        /// <summary>
        /// 持续时长
        /// </summary>
        public long duration { get; set; }

        /// <summary>
        /// 操作系统相关参数
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public Dictionary<string, string> os_params { get; set; }
    }
}
