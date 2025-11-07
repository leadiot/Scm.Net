using Com.Scm.Dao;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Log
{
    /// <summary>
    /// 三方登录日志
    /// </summary>
    [SugarTable("scm_log_oauth")]
    public class LogOidcDao : ScmDataDao
    {
        /// <summary>
        /// 登录标识
        /// </summary>
        [Required]
        [StringLength(32)]
        public string code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(128)]
        public string state { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(128)]
        public string scope { get; set; }

        /// <summary>
        /// 服务
        /// </summary>
        public string provider { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(64)]
        public string user { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [StringLength(32)]
        public string name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(256)]
        public string avatar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(256)]
        public string access_token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(256)]
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
        public string err_code { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        [StringLength(128)]
        public string err_msg { get; set; }

        public int qty { get; set; }

        public int handle { get; set; }

        public bool IsExpired(DateTime time)
        {
            return TimeUtils.GetUnixTime(time) > expires_in;
        }
    }
}