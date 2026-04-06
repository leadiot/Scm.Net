using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Ur.User.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 展示姓名
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string pass { get; set; }

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

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public long login_time { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int login_count { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public string last_time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<long> organize_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<long> position_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<long> group_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<long> role_list { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmUserDataEnum data { get; set; }
    }
}
