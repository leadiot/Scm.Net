using Com.Scm.Dao.User;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 终端授权
    /// </summary>
    [SqlSugar.SugarTable("scm_ur_user_token")]
    public class UserTokenDao : ScmUserDataDao
    {
        /// <summary>
        /// 授权类型（可以是设备、软件或其它）
        /// </summary>
        public ScmUserTokenTypeEnum types { get; set; }

        /// <summary>
        /// 授权名称
        /// </summary>
        [Required]
        [StringLength(32)]
        public string names { get; set; }

        /// <summary>
        /// 凭证
        /// </summary>
        [StringLength(32)]
        public string token { get; set; }

        /// <summary>
        /// 刷新凭证
        /// </summary>
        [StringLength(32)]
        public string refresh { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long expired { get; set; }

        /// <summary>
        /// 授权时间
        /// </summary>
        public long auth_time { get; set; }
    }
}
