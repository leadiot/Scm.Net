using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 审批单据
    /// </summary>
    [SugarTable("scm_flow_data_header")]
    public class ScmFlowDataHeaderDao : ScmDataDao
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public long flow_id { get; set; }

        /// <summary>
        /// 结点ID
        /// </summary>
        public long node_id { get; set; }

        /// <summary>
        /// 表格名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string table { get; set; }

        /// <summary>
        /// 单据ID
        /// </summary>
        public long order_id { get; set; }

        /// <summary>
        /// 单据编码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string order_codes { get; set; }

        /// <summary>
        /// 单据图标
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string icon { get; set; }

        /// <summary>
        /// 审批标题
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string title { get; set; }

        /// <summary>
        /// 单据地址
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string url { get; set; }

        /// <summary>
        /// 审批说明
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string remark { get; set; }

        /// <summary>
        /// 已处理结点
        /// </summary>
        [SugarColumn(Length = 512, IsNullable = true, IsJson = true)]
        public List<long> nodes { get; set; }

        public void AddNode(long nodeId)
        {
            if (nodes == null)
            {
                nodes = new List<long>();
            }
            nodes.Add(nodeId);
        }
    }
}