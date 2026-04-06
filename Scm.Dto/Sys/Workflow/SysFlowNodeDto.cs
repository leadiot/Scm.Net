using Com.Scm.Dto;
using Com.Scm.Workflow;

namespace Com.Scm.Sys.Workflow
{
    public class SysFlowNodeDto : ScmDataDto
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
        public List<string> user_list { get; set; }
        /// <summary>
        /// 角色列表
        /// </summary>
        public List<string> role_list { get; set; }
        /// <summary>
        /// 节点参数
        /// </summary>
        public List<string> args { get; set; }
    }
}
