using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Samples.Demo
{
    /// <summary>
    /// 演示对象DAO
    /// </summary>
    [SqlSugar.SugarTable("samples_demo")]
    public class DemoDao : ScmDataDao, ISystemDao, IDeleteDao
    {
        /// <summary>
        /// 选项
        /// </summary>
        public long option_id { get; set; }
        /// <summary>
        /// 系统编码（全局编码，用于系统时API交换）
        /// 格式：DEMO00000001
        /// </summary>
        [StringLength(16)]
        public string codes { get; set; }
        /// <summary>
        /// 客户编码（客户自定义编码，具有业务含义）
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }
        /// <summary>
        /// 系统名称（简称，支持搜索）
        /// </summary>
        [StringLength(32)]
        public string names { get; set; }
        /// <summary>
        /// 客户名称（全称）
        /// </summary>
        [StringLength(128)]
        public string namec { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(32)]
        public string phone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        public string remark { get; set; }

        /// <summary>
        /// 系统记录标识（不是必需）
        /// </summary>
        public ScmSystemEnum row_system { get; set; }

        /// <summary>
        /// 数据删除标识（不是必需）
        /// </summary>
        public ScmDeleteEnum row_delete { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            // 新增时，自动生成系统编码
            codes = UidUtils.NextCodes("samples_demo");
            // 新增时，自动生成系统名称
            if (string.IsNullOrWhiteSpace(names))
            {
                names = namec;
            }
        }
    }
}
