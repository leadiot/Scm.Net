using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Log
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_log_otp")]
    public class LogOtpDao : ScmDataDao
    {
        /// <summary>
        /// 身份标识
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string key { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [Required]
        public long sms_id { get; set; }

        /// <summary>
        /// 模板代码（冗余）
        /// </summary>
        [StringLength(16)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string sms_codec { get; set; }

        /// <summary>
        /// 终端类型
        /// </summary>
        [Required]
        public OtpTypesEnum types { get; set; }

        /// <summary>
        /// 终端号码
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string code { get; set; }

        /// <summary>
        /// 请求序列
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string seq { get; set; }

        /// <summary>
        /// 校验码
        /// </summary>
        [StringLength(8)]
        [SugarColumn(Length = 8, IsNullable = true)]
        public string pass { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string content { get; set; }

        /// <summary>
        /// 发送次数
        /// </summary>
        [Required]
        public int send_qty { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [Required]
        public long send_time { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Required]
        public long expired { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public ScmHandleEnum handle { get; set; }

        /// <summary>
        /// 发送结果
        /// </summary>
        public ScmResultEnum result { get; set; }

        /// <summary>
        /// 核验次数
        /// </summary>
        public int verify { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsExpired(DateTime time)
        {
            return TimeUtils.GetUnixTime(time) > expired;
        }
    }
}
