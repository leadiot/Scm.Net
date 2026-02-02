using Com.Scm.Dao;
using Com.Scm.Workflow;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Workflow
{
    /// <summary>
    /// 工作流节点
    /// </summary>
    [SugarTable("scm_flow_node")]
    public class FlowNodeDao : ScmDataDao
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public long flow_id { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        public FlowNodeTypesEnum types { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string names { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 上级节点
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 用户列表
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true, IsJson = true)]
        public List<string> user_list { get; set; }
        /// <summary>
        /// 角色列表
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true, IsJson = true)]
        public List<string> role_list { get; set; }
        /// <summary>
        /// 节点参数
        /// </summary>
        [SugarColumn(Length = 256, IsNullable = true, IsJson = true)]
        public List<string> args { get; set; }
    }
}
