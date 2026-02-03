using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.App
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_res_app")]
    public class ScmResAppDao : ScmDataDao, IResDao
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public long org_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codes { get; set; }

        /// <summary>
        /// 应用代码
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codec { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string namec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string names { get; set; }

        /// <summary>
        /// 应用说明
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string remark { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            if (string.IsNullOrWhiteSpace(names))
            {
                names = namec;
            }
        }

        public string GetCode()
        {
            return codec;
        }

        public string GetName()
        {
            return names ?? namec;
        }

        public string GetNames()
        {
            return names;
        }

        public string GetNamec()
        {
            return namec;
        }
    }
}