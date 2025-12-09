using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Samples.PoHeader.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Samples.PoHeader.Dao
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("samples_po_header")]
    public class SamplesPoHeaderDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public SamplesPoTypesEnum types { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        [StringLength(16)]
        public string codes { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }

        /// <summary>
        /// 单品需求数量
        /// </summary>
        public int item_need_qty { get; set; }

        /// <summary>
        /// 单品实际数量
        /// </summary>
        public int item_real_qty { get; set; }

        /// <summary>
        /// 单件需求数量
        /// </summary>
        public int unit_need_qty { get; set; }

        /// <summary>
        /// 单件实际数量
        /// </summary>
        public int unit_real_qty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(128)]
        public string remark { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public ScmWfaStatusEnum wfa_status { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            this.codes = UidUtils.NextCodes("samples_po_header", (int)types);
        }
    }
}