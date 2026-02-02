using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_fes_org")]
    public class ScmNasOrgDao : ScmDataDao
    {
        /// <summary>
        /// 组织代码
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codec { get; set; }

        /// <summary>
        /// 组织简称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string names { get; set; }

        /// <summary>
        /// 组织全称
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string namec { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            if (string.IsNullOrWhiteSpace(names))
            {
                names = namec;
            }
        }
    }
}