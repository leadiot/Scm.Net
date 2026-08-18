using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Sms
{
    [SugarTable("scm_sys_sms_thread")]
    public class ScmSysSmsThreadDao : ScmUserDataDao
    {
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string address { get; set; }

        /// <summary>
        /// 会话名称
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string name { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int color { get; set; }
    }
}
