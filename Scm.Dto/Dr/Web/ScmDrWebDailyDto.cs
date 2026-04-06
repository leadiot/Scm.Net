using Com.Scm.Dto;

namespace Com.Scm.Dr.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmDrWebDailyDto : ScmDataDto
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