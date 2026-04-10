using Com.Scm.Dao;
using Com.Scm.Dao.User;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 终端
    /// </summary>
    [SugarTable("scm_ur_terminal")]
    public class ScmUrTerminalDao : ScmUserDataDao, IResDao
    {
        /// <summary>
        /// 终端类型
        /// </summary>
        [Required]
        public ScmClientTypeEnum types { get; set; }

        /// <summary>
        /// 终端代码
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codes { get; set; }

        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 终端名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string names { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string namec { get; set; }

        /// <summary>
        /// 终端口令
        /// </summary>
        [StringLength(16)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string pass { get; set; }

        /// <summary>
        /// 终端图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 绑定状态
        /// </summary>
        public ScmBoolEnum binded { get; set; }

        /// <summary>
        /// 终端授权
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string access_token { get; set; }

        /// <summary>
        /// 刷新授权
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string refresh_token { get; set; }

        /// <summary>
        /// 过期时间(UTC时间)
        /// </summary>
        public long expired { get; set; }

        /// <summary>
        /// MAC地址
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string mac { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string os { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string dn { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string dm { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public ScmBoolEnum online { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string remark { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            this.codes = UidUtils.NextCodes("scm_ur_terminal", (int)this.types);
            if (string.IsNullOrWhiteSpace(this.names))
            {
                this.names = this.namec;
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

        public bool IsExpired()
        {
            return TimeUtils.GetUnixTime(true) > expired;
        }
    }
}