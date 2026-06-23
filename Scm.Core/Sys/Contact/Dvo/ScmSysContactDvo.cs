using Com.Scm.Dvo;

namespace Com.Scm.Sys.Contact.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmSysContactDvo : ScmDataDvo
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string first_name { get; set; }

        /// <summary>
        /// 中间名
        /// </summary>
        public string middle_name { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string last_name { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string name_prefix { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string name_suffix { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string company { get; set; }

        /// <summary>
        /// 抬头
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        public string website { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string note { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string birthday { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string photo_thumb_uri { get; set; }

        /// <summary>
        /// 头像原图
        /// </summary>
        public string photo_uri { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public List<Dictionary<string, string>> emails { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public List<Dictionary<string, string>> phones { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public List<Dictionary<string, string>> addresses { get; set; }

        /// <summary>
        /// 即时通讯地址
        /// </summary>
        public List<Dictionary<string, string>> im_addresses { get; set; }

        /// <summary>
        /// 操作系统相关参数
        /// </summary>
        public Dictionary<string, string> os_params { get; set; }
    }
}
