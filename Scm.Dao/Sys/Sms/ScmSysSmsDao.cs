using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Sms
{
    [SugarTable("scm_sys_sms")]
    public class ScmSysSmsDao : ScmUserDataDao
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string address { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string body { get; set; }

        /// <summary>
        /// 发送日期
        /// </summary>
        public long date { get; set; }

        /// <summary>
        /// 短信类型
        /// </summary>
        public ScmSmsTypeEnum type { get; set; }

        /// <summary>
        /// 操作系统相关参数
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public Dictionary<string, string> os_params { get; set; }
    }
}
