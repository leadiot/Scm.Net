using Com.Scm.Dao;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 
    /// </summary>
    [SqlSugar.SugarTable("scm_ur_role_auth")]
    public class RoleMenuDao : ScmDataDao
    {
        /// <summary>
        /// 角色编号
        /// </summary>
        [Required]
        public long role_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 菜单编号
        /// </summary>
        public long auth_id { get; set; }

        /// <summary>
        /// 授权类型1=角色-菜单
        /// 暂时没有使用
        /// </summary>
        [Required]
        public ScmRoleAuthTypesEnum types { get; set; }
    }
}
