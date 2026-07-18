using Com.Scm.Enums;

namespace Com.Scm.Dao
{
    public interface IApprovalDao
    {
        public long id { get; set; }

        /// <summary>
        /// 单据编码
        /// </summary>
        public string codes { get; set; }

        /// <summary>
        /// 单据名称
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 审批状态
        /// </summary>
        public ScmWfaStatusEnum wfa_status { get; set; }
    }
}
