using Com.Scm.Dao;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 群组表
    /// </summary>
    [SugarTable("scm_ur_group")]
    public class GroupDao : ScmDataDao, IResDao
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string namec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            codes = UidUtils.NextCodes("scm_ur_group");
            if (string.IsNullOrEmpty(names))
            {
                names = namec;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareUpdate(long userId)
        {
            base.PrepareUpdate(userId);
            if (string.IsNullOrEmpty(names))
            {
                names = namec;
            }
        }

        public string GetCode()
        {
            return codes;
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
