using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Log.User.Dvo
{
    public class LogUserDvo : ScmDvo
    {
        /// <summary>
        /// 终端类型
        /// </summary>
        public ScmClientTypeEnum client { get; set; }
        public string client_names { get; set; }
        /// <summary>
        /// 登录模式
        /// </summary>
        public ScmLoginModeEnum mode { get; set; }
        public string mode_names { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public long time { get; set; }
        /// <summary>
        /// 登录地址
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// 登录结果
        /// </summary>
        public ScmBoolEnum result { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { get; set; }
    }
}
