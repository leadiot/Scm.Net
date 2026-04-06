using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur
{
    public class RoleConflictDto : ScmDataDto
    {
        /// <summary>
        /// 角色A
        /// </summary>
        public long rolea_id { get; set; }

        public string rolea_name { get; set; }

        /// <summary>
        /// 角色B
        /// </summary>
        public long roleb_id { get; set; }

        public string roleb_name { get; set; }

        /// <summary>
        /// 互斥说明
        /// </summary>
        [Required]
        [StringLength(256)]
        public string remark { get; set; }
    }
}
