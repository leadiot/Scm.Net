using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Samples.Book.Enums;
using Com.Scm.Utils;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Samples.Book.Dao
{
    /// <summary>
    /// 演示对象DAO
    /// </summary>
    [SqlSugar.SugarTable("samples_book")]
    public class BookDao : ScmDataDao, ISystemDao, IDeleteDao, IApprovalDao
    {
        /// <summary>
        /// 书籍类型
        /// </summary>
        public BookTypesEnum types { get; set; }
        /// <summary>
        /// 系统编码（全局编码，用于系统时API交换）
        /// 格式：DEMO00000001
        /// </summary>
        [StringLength(16)]
        public string codes { get; set; }
        /// <summary>
        /// 书籍编码（书籍自定义编码，具有业务含义）
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }
        /// <summary>
        /// 系统名称（简称，支持搜索）
        /// </summary>
        [StringLength(32)]
        public string names { get; set; }
        /// <summary>
        /// 书籍名称（全称）
        /// </summary>
        [StringLength(128)]
        public string namec { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        [StringLength(32)]
        public string barcode { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [StringLength(32)]
        public string image { get; set; }
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

        /// <summary>
        /// 审批状态（不是必需）
        /// </summary>
        public ScmWfaStatusEnum wfa_status { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            // 新增时，自动生成系统编码
            codes = UidUtils.NextCodes("samples_book");
            // 新增时，自动生成系统名称
            if (string.IsNullOrWhiteSpace(names))
            {
                names = namec;
            }
        }
    }
}
