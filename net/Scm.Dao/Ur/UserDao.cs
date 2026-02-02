using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace Com.Scm.Ur
{
    /// <summary>
    /// 用户表
    /// </summary>
    [SugarTable("scm_ur_user")]
    public class UserDao : ScmDataDao, ISystemDao, IDeleteDao, IResDao
    {
        /// <summary>
        /// 
        /// </summary>
        public ScmUserTypesEnum types { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codes { get; set; }

        /// <summary>
        /// 用户编码（对应客户系统编码）
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 系统姓名，如真实姓名
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string names { get; set; }

        /// <summary>
        /// 展示姓名，如用户昵称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string namec { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string pass { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string avatar { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string cellphone { get; set; }
        /// <summary>
        /// 固话
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string telephone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public ScmSexEnum sex { get; set; }

        /// <summary>
        /// OTP是否启用
        /// </summary>
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmRowStatusEnum otp_status { get; set; }
        /// <summary>
        /// OTP Secret
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string otp_secret { get; set; }
        /// <summary>
        /// Otp启用时间
        /// </summary>
        public long otp_time { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string remark { get; set; }

        #region 登录记录
        /// <summary>
        /// 登录次数
        /// </summary>
        [Required]
        public int login_count { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public long login_time { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public long last_time { get; set; }
        #endregion

        #region 安全检测
        /// <summary>
        /// 错误次数
        /// </summary>
        public int error_count { get; set; }

        /// <summary>
        /// 下次登录时间
        /// </summary>
        public long next_time { get; set; }
        #endregion

        #region 数据权限
        /// <summary>
        /// 数据权限
        /// </summary>
        public ScmUserDataEnum data { get; set; }
        /// <summary>
        /// 首页展示
        /// </summary>
        public ScmUserHomeTypesEnum home { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmRowSystemEnum row_system { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmRowDeleteEnum row_delete { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            codes = UidUtils.NextCodes("scm_ur_user", (int)types);
            row_delete = ScmRowDeleteEnum.No;
            if (string.IsNullOrEmpty(names))
            {
                names = namec;
            }
            login_time = 0;
            last_time = 0;
            next_time = 0;
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

        /// <summary>
        /// 
        /// </summary>
        public void EncodePass()
        {
            pass = ScmUtils.EncodePass(pass);
        }

        /// <summary>
        /// 
        /// </summary>
        public void DecodePass()
        {
            pass = ScmUtils.DecodePass(pass);
        }

        public void UseDefaultPass()
        {
            this.pass = "";
        }

        public void UseDefaultAvatar()
        {
            this.avatar = "0.png";
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="length"></param>
        public void GenerateSecret(int length = 16)
        {
            byte[] randomBytes = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var result = SecUtils.AesEncrypt(randomBytes);
            otp_secret = Convert.ToBase64String(result);
        }

        public byte[] DecodeSecret()
        {
            if (string.IsNullOrEmpty(otp_secret))
            {
                return null;
            }

            var bytes = Convert.FromBase64String(otp_secret);
            return SecUtils.AesDecrypt(bytes);
        }
    }
}