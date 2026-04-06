using Com.Scm.Dvo;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur.Role.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 角色父节点
        /// </summary>
        public long pid { get { return _pid; } set { _pid = value; } }
        private long _pid;
        /// <summary>
        /// 
        /// </summary>
        public long ParentId { get { return _pid; } set { _pid = value; } }

        /// <summary>
        /// 角色层级
        /// </summary>
        [Required]
        public int lv { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string remark { get; set; }
    }
}
