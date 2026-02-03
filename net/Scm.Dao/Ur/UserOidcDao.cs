using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 三方登录
    /// </summary>
    [SugarTable("scm_ur_user_oidc")]
    public class UserOidcDao : ScmUserDataDao
    {
        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 外部应用
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string provider { get; set; }

        /// <summary>
        /// 授权ID
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string oauth_id { get; set; }

        /// <summary>
        /// 用户代码
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string user { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string name { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string avatar { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int qty { get; set; }
    }
}