using Com.Scm.Flow;
using Com.Scm.Sys;
using Com.Scm.Workflow;
using Com.Scm.Workflow.Dto;

namespace Com.Scm
{
    public interface IFlowService
    {
        MatchAction IsMatch { get; set; }

        UserAction GetUserList { get; set; }

        UpdateStatusAction UpdateWfaStatus { get; set; }

        /// <summary>
        /// 获取流程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<FlowInfoDto> GetFlowinfoAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ScmFlowDataHeaderDao> GetFlowdataAsync(long id);

        /// <summary>
        /// 获取单据的审批流程信息
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appCode"></param>
        /// <returns></returns>
        Task<ScmFlowOrderDao> GetFlowOrderAsync(string orderCode);

        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="flowDao"></param>
        /// <param name="orderId"></param>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> StartFlow(ScmFlowOrderDao flowDao, long orderId, string orderCode, long userId);
    }
}
