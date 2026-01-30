using Com.Scm.Dvo;

namespace Com.Scm.Nas.Cfg.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class NasCfgFolderDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public long terminal_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// ¼ÇÂ¼ID
        /// </summary>
        public long res_id { get; set; }
    }
}