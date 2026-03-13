using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Scm.Ur
{
    /// <summary>
    /// 终端
    /// </summary>
    public class ScmUrTerminalDvo : ScmDataDvo
    {
        /// <summary>
        /// 终端类型
        /// </summary>
        public ScmClientTypeEnum types { get; set; }

        public string types_name { get; set; }

        /// <summary>
        /// 终端代码
        /// </summary>
        public string codes { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 终端授权
        /// </summary>
        public string pass { get; set; }

        /// <summary>
        /// 绑定状态
        /// </summary>
        public ScmBoolEnum binded { get; set; }

        /// <summary>
        /// MAC地址
        /// </summary>
        public string mac { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string os { get; set; }
    }
}