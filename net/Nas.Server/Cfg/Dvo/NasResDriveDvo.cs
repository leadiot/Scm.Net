using Com.Scm.Dvo;

namespace Com.Scm.Nas.Cfg.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class NasResDriveDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public long terminal_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long folder_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string path { get; set; }
    }
}