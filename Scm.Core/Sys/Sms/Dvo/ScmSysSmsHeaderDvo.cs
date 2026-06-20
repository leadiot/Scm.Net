using Com.Scm.Dvo;

namespace Com.Scm.Sys.Sms.Dvo
{
    public class ScmSysSmsHeaderDvo : ScmDataDvo
    {
        /// <summary>
        /// 电话
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 发送日期
        /// </summary>
        public long date { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public int color { get; set; }
    }
}
