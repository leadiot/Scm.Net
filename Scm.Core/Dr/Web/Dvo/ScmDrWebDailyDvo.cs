using Com.Scm.Dvo;

namespace Com.Scm.Dr
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmDrWebDailyDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public string day { get; set; }

        /// <summary>
        /// 页面访问量
        /// </summary>
        public int pv { get; set; }
        /// <summary>
        /// 用户访问量
        /// </summary>
        public int uv { get; set; }
    }
}