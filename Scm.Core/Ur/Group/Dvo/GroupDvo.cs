using Com.Scm.Dvo;

namespace Com.Scm.Ur.Group.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string names { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string namec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long pid { get { return _pid; } set { _pid = value; } }
        /// <summary>
        /// 
        /// </summary>
        public long ParentId { get { return _pid; } set { _pid = value; } }
        private long _pid;

        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
    }
}
