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
        /// 应用简介
        /// </summary>
        public string content { get; set; }
    }
}
