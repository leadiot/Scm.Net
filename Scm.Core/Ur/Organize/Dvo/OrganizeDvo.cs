using Com.Scm.Dvo;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur.Organize.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class OrganizeDvo : ScmDataDvo
    {
        /// <summary>
        /// 父节点
        /// </summary>
        public long pid { get { return _pid; } set { _pid = value; } }
        /// <summary>
        /// 
        /// </summary>
        public long ParentId { get { return _pid; } set { _pid = value; } }
        private long _pid;


        /// <summary>
        /// 部门名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 部门层级
        /// </summary>
        public int lv { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int od { get; set; }

        /// <summary>
        /// 主管人员
        /// </summary>
        public long owner_id { get; set; }

        /// <summary>
        /// 主管人员(冗余)
        /// </summary>
        public string owner_names { get; set; }
    }
}
