using Com.Scm.Dao.User;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Scm.Ur
{
    /// <summary>
    /// 终端
    /// </summary>
    [SugarTable("scm_ur_terminal")]
    public class ScmUrTerminalDao : ScmUserDataDao
    {
        /// <summary>
        /// 终端类型
        /// </summary>
        [Required]
        public ScmClientTypeEnum types { get; set; }

        /// <summary>
        /// 终端代码
        /// </summary>
        [Required]
        [StringLength(16)]
        public string codes { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        [StringLength(32)]
        public string names { get; set; }

        /// <summary>
        /// 终端口令
        /// </summary>
        [StringLength(16)]
        public string pass { get; set; }

        /// <summary>
        /// 绑定状态
        /// </summary>
        public ScmBoolEnum binded { get; set; }

        /// <summary>
        /// 终端授权
        /// </summary>
        [Required]
        [StringLength(32)]
        public string access_token { get; set; }

        /// <summary>
        /// 刷新授权
        /// </summary>
        [Required]
        [StringLength(32)]
        public string refresh_token { get; set; }

        /// <summary>
        /// 过期时间(UTC时间)
        /// </summary>
        public long expires { get; set; }

        /// <summary>
        /// MAC地址
        /// </summary>
        public string mac { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string os { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            this.codes = UidUtils.NextCodes("scm_ur_terminal", (int)this.types);
        }
    }
}