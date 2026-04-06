using Com.Scm.Dao;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Log
{
    /// <summary>
    /// 三方登录日志
    /// </summary>
    [SugarTable("scm_log_oidc")]
    public class LogOidcDao : ScmDataDao
    {
        /// <summary>
        /// 登录标识
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string state { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string scope { get; set; }

        /// <summary>
        /// 服务
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string provider { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string user { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string avatar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string access_token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string refresh_token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Required]
        public long expires_in { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        [StringLength(8)]
        [SugarColumn(Length = 8, IsNullable = true)]
        public string err_code { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string err_msg { get; set; }

        public int qty { get; set; }

        public int handle { get; set; }

        public bool IsExpired(DateTime time)
        {
            return TimeUtils.GetUnixTime(time) > expires_in;
        }
    }
}