using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 终端
    /// </summary>
    public class ScmUrTerminalDto : ScmDataDto
    {
        /// <summary>
        /// 终端类型
        /// </summary>
        [Required]
        public ScmClientTypeEnum types { get; set; }

        /// <summary>
        /// 终端代码
        /// </summary>
        public string codes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        [Required]
        [StringLength(32)]
        public string namec { get; set; }

        /// <summary>
        /// 终端口令
        /// </summary>
        [StringLength(16)]
        public string pass { get; set; }

        /// <summary>
        /// 终端图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 绑定状态
        /// </summary>
        public ScmBoolEnum binded { get; set; }

        /// <summary>
        /// 终端授权
        /// </summary>
        [StringLength(32)]
        public string access_token { get; set; }

        /// <summary>
        /// 刷新授权
        /// </summary>
        [StringLength(32)]
        public string refresh_token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long expired { get; set; }

        /// <summary>
        /// MAC地址
        /// </summary>
        public string mac { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string os { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public ScmBoolEnum online { get; set; }

        [StringLength(256)]
        public string remark { get; set; }

        public void GenIcon()
        {
            this.icon = "ms-devices";
            switch (types)
            {
                case ScmClientTypeEnum.Web:
                    this.icon = "ms-laptop";
                    break;
                case ScmClientTypeEnum.Windows:
                    this.icon = "ms-computer";
                    break;
                case ScmClientTypeEnum.Android:
                    this.icon = "ms-smartphone";
                    break;
                case ScmClientTypeEnum.iOS:
                    this.icon = "ms-smartphone";
                    break;
                case ScmClientTypeEnum.SmallApp:
                    this.icon = "ms-tablet";
                    break;
                default:
                    this.icon = "ms-devices";
                    break;
            }
        }
    }
}