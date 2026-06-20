using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Contact
{
    [SugarTable("scm_sys_contact")]
    public class ScmSysContactDao : ScmUserDataDao
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string name { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        [SugarColumn(Length = 32, IsNullable = true)]
        public string firstName { get; set; }

        /// <summary>
        /// 中间名
        /// </summary>
        [SugarColumn(Length = 32, IsNullable = true)]
        public string middleName { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        [SugarColumn(Length = 32, IsNullable = true)]
        public string lastName { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        [SugarColumn(Length = 8, IsNullable = true)]
        public string namePrefix { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        [SugarColumn(Length = 8, IsNullable = true)]
        public string nameSuffix { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string nickname { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true)]
        public string company { get; set; }

        /// <summary>
        /// 抬头
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string title { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [SugarColumn(Length = 64, IsNullable = true)]
        public string department { get; set; }

        /// <summary>
        /// 网站
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true)]
        public string website { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true)]
        public string note { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [SugarColumn(Length = 10, IsNullable = true)]
        public string birthday { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true)]
        public string photoThumbUri { get; set; }

        /// <summary>
        /// 头像原图
        /// </summary>
        [SugarColumn(Length = 128, IsNullable = true)]
        public string photoUri { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public List<KvItem<string, string>> emails { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public List<KvItem<string, string>> phones { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public List<KvItem<string, string>> addresses { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public List<KvItem<string, string>> imAddresses { get; set; }

        /// <summary>
        /// 操作系统相关参数
        /// </summary>
        [SugarColumn(Length = 1024, IsNullable = true, IsJson = true)]
        public Dictionary<string, string> os_params { get; set; }
    }
}