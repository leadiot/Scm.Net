using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_fes_ext")]
    public class ScmNasExtDao : ScmDataDao
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public ScmFileTypeEnum types { get; set; }

        /// <summary>
        /// 后缀代码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 后缀名称
        /// </summary>
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string namec { get; set; }

        /// <summary>
        /// 文件签名
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string sign { get; set; }

        /// <summary>
        /// 组织ID
        /// </summary>
        public long org_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long app_id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string remark { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            if (string.IsNullOrWhiteSpace(namec))
            {
                namec = codec + " 文件";
            }
        }
    }
}