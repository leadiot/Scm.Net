using Com.Scm.Enums;
using Com.Scm.Utils;

namespace Com.Scm.Terminal
{
    public class ScmTerminalToken
    {
        public long id { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        public ScmClientTypeEnum types { get; set; }

        /// <summary>
        /// 终端代码
        /// </summary>
        public string codes { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 绑定状态
        /// </summary>
        public ScmBoolEnum binded { get; set; }

        /// <summary>
        /// 终端授权
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 刷新授权
        /// </summary>
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

        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>
        public string user_codes { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string user_names { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        /// <returns></returns>
        public bool IsExpreid()
        {
            return TimeUtils.GetUnixTime() > 0;
        }
    }
}
