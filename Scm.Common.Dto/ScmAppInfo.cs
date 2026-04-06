using Com.Scm.Dto;

namespace Com.Scm
{
    public class ScmAppInfo : ScmDto
    {
        /// <summary>
        /// 应用类型
        /// </summary>
        public int types { get; set; }

        /// <summary>
        /// 应用代码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 项目地址
        /// </summary>
        public string project { get; set; }

        /// <summary>
        /// 网站地址
        /// </summary>
        public string website { get; set; }

        /// <summary>
        /// 应用简介
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 群聊
        /// </summary>
        public string qchat { get; set; }
    }
}
