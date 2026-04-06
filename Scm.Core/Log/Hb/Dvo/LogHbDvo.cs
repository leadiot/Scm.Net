using Com.Scm.Dvo;

namespace Com.Scm.Log.Hb.Dvo
{
    public class LogHbDvo : ScmDvo
    {
        /// <summary>
        /// 网络地址
        /// </summary>
        public string host { get; set; }
        /// <summary>
        /// 主机名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 心跳时间
        /// </summary>
        public long time { get; set; }
    }
}
