using Com.Scm.Dao;
using Com.Scm.Log;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Otp
{
    /// <summary>
    /// 消息模板
    /// </summary>
    [SugarTable("scm_res_otp")]
    public class ScmResOtpDao : ScmDataDao
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        public OtpTypesEnum types { get; set; }

        /// <summary>
        /// 模板代码
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codes { get; set; }

        /// <summary>
        /// 模板代码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string names { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string namec { get; set; }

        /// <summary>
        /// 标题模板
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string head { get; set; }

        /// <summary>
        /// 内容模板
        /// </summary>
        [StringLength(512)]
        [SugarColumn(Length = 512, IsNullable = true)]
        public string body { get; set; }

        /// <summary>
        /// 声明模板
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string foot { get; set; }

        /// <summary>
        /// 文件模板
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string file { get; set; }
    }
}
