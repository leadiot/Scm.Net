using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Sms
{
    [SugarTable("scm_sys_sms_header")]
    public class ScmSysSmsHeaderDao : ScmUserDataDao
    {
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string phone { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string name { get; set; }

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
        public long time { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int color { get; set; }
    }
}
