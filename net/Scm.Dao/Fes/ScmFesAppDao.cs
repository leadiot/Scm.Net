using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Fes
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_fes_app")]
    public class ScmFesAppDao : ScmDataDao
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public long org_id { get; set; }

        /// <summary>
        /// 应用代码
        /// </summary>
        [StringLength(16)]
        public string codec { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [StringLength(64)]
        public string namec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(64)]
        public string names { get; set; }

        /// <summary>
        /// 应用说明
        /// </summary>
        [StringLength(128)]
        public string remark { get; set; }

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