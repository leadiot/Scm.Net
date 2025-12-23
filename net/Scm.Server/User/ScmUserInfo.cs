using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.User
{
    public class ScmUserInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public ScmUserTypesEnum types { get; set; }
        /// <summary>
        /// 系统代码
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 用户编码（对应客户系统编码）
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 系统姓名，如真实姓名
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 展示姓名，如用户昵称
        /// </summary>
        [Required]
        public string namec { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string cellphone { get; set; }
        /// <summary>
        /// 固话
        /// </summary>
        public string telephone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public ScmSexEnum sex { get; set; }
    }
}
