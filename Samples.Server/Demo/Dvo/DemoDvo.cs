using Com.Scm.Dvo;

namespace Com.Scm.Samples.Demo.Dvo
{
    public class DemoDvo : ScmDataDvo
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 客户编码
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public string names { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string namec { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
}
