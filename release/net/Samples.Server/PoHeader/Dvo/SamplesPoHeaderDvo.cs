using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Samples.PoHeader.Enums;

namespace Com.Scm.Samples.PoHeader.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SamplesPoHeaderDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public SamplesPoTypesEnum types { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        public string codes { get; set; }

        /// <summary>
        /// 采购单号
        /// </summary>
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
        public string remark { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public ScmWfaStatusEnum wfa_status { get; set; }
    }
}