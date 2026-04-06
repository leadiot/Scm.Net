using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Flow;
using Com.Scm.Sys;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Com.Scm.Workflow;
using Com.Scm.Workflow.Dto;
using SqlSugar;

namespace Com.Scm.Service
{
    public class ScmFlowService : IFlowService
    {
        private EnvConfig _EnvConfig;
        private ISqlSugarClient _SqlClient;

        public ScmFlowService(ISqlSugarClient sqlClient, EnvConfig envConfig)
        {
            _SqlClient = sqlClient;
            _EnvConfig = envConfig;
        }

        /// <summary>
        /// 匹配方法
        /// </summary>
        public MatchAction IsMatch { get; set; }

        /// <summary>
        /// 获取用户操作
        /// </summary>
        public UserAction GetUserList { get; set; }

        /// <summary>
        /// 更新审批状态
        /// </summary>
        public UpdateStatusAction UpdateWfaStatus { get; set; }

        /// <summary>
        /// 读取流程配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FlowInfoDto> GetFlowinfoAsync(long id)
        {
            var json = await _EnvConfig.ReadFileAsync("flow", id + ".json");
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return json.AsJsonObject<FlowInfoDto>();
        }

        public async Task<ScmFlowDataHeaderDao> GetFlowdataAsync(long id)
        {
            return await _SqlClient.Queryable<ScmFlowDataHeaderDao>().FirstAsync(a => a.id == id);
        }

        /// <summary>
        /// 获取单据的审批流程信息
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public Task<ScmFlowOrderDao> GetFlowOrderAsync(string orderCode)
        {
            return _SqlClient.Queryable<ScmFlowOrderDao>()
                .Where(a => a.codec == orderCode && a.row_status == Com.Scm.Enums.ScmRowStatusEnum.Enabled)
                .FirstAsync();
        }

        /// <summary>
        /// 创建一个流程
        /// </summary>
        /// <param name="flowDao"></param>
        /// <param name="orderId"></param>
        /// <param name="orderCode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> StartFlow(ScmFlowOrderDao flowDao, long orderId, string orderCode, long userId)
        {
            var dao = await _SqlClient.Queryable<ScmFlowDataHeaderDao>()
                .Where(a => a.flow_id == flowDao.id && a.order_id == orderId && a.row_status == ScmRowStatusEnum.Enabled)
                .FirstAsync();

            if (dao != null)
            {
                // 流程已经存在
                return true;
            }

            // 创建流程单据
            dao = new ScmFlowDataHeaderDao();
            dao.flow_id = flowDao.id;
            dao.node_id = 0;
            dao.url = flowDao.url;
            dao.order_id = orderId;
            dao.order_codes = orderCode;
            dao.PrepareCreate(userId);
            await _SqlClient.Insertable(dao).ExecuteCommandAsync();

            // 记录用户审批日志
            await SaveDataResult(dao, "发起审批流程", userId);

            // 获取流程信息
            var flowInfo = await GetFlowinfoAsync(dao.flow_id);
            if (flowInfo == null)
            {
                await SaveDataRemark(dao, $"流程配置文件{dao.flow_id}.json文件内容为空！");
                return false;
            }

            var curNode = flowInfo.nodeConfig;
            if (curNode == null)
            {
                await SaveDataRemark(dao, $"流程配置异常！");
                return false;
            }

            // 查找下一个节点
            return await CheckNextNode(dao, curNode.childNode, userId);
        }

        /// <summary>
        /// 审批通过
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> AcceptAsync(long id, string comment, long userId)
        {
            // 获取流程单据
            var dataDao = await _SqlClient.Queryable<ScmFlowDataHeaderDao>()
                .FirstAsync(a => a.id == id && a.row_status == ScmRowStatusEnum.Enabled);
            if (dataDao == null)
            {
                LogUtils.Error($"表scm_flow_data_header找不到id为{id}的流程数据！");
                return false;
            }
            dataDao.AddNode(id);

            // 记录用户审批结果
            var detailDao = await _SqlClient.Queryable<ScmFlowDataDetailDao>()
                .FirstAsync(a => a.header_id == id && a.node_id == dataDao.node_id && a.user_id == userId);
            if (detailDao == null)
            {
                await SaveDataRemark(dataDao, $"表scm_flow_data_detail找不到user_id为{userId}的审批人员！");
                return false;
            }
            detailDao.wfa_result = FlowDataResultEnum.Accept;
            detailDao.comment = comment;
            detailDao.PrepareUpdate(userId);
            await _SqlClient.Updateable(detailDao).ExecuteCommandAsync();

            // 记录用户审批日志
            await SaveDataResult(dataDao, comment, userId);

            // 获取流程信息
            var flowInfo = await GetFlowinfoAsync(dataDao.flow_id);
            if (flowInfo == null)
            {
                await SaveDataRemark(dataDao, $"流程配置文件{dataDao.flow_id}.json文件内容为空！");
                return false;
            }

            // 获取当前节点
            var curNode = flowInfo.GetNode(dataDao.nodes);
            if (curNode == null)
            {
                await SaveDataRemark(dataDao, $"审批流程找不到id为{dataDao.node_id}的节点！");
                return false;
            }

            // 结束节点
            if (curNode.IsEnd())
            {
                return await ProcessEndNode(dataDao, curNode, userId);
            }

            // 查找下一个节点
            return await CheckNextNode(dataDao, curNode.childNode, userId);
        }

        /// <summary>
        /// 检查下一个节点
        /// </summary>
        /// <param name="dataDao"></param>
        /// <param name="subNode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<bool> CheckNextNode(ScmFlowDataHeaderDao dataDao, FlowNodeDto subNode, long userId)
        {
            if (subNode == null)
            {
                await SaveDataRemark(dataDao, $"流程数据{dataDao.id}找不到合适的下级节点！");
                return false;
            }

            if (subNode.type == FlowNodeTypesEnum.Approve)
            {
                // 审批节点
                return await ProcessApproveNode(dataDao, subNode, userId);
            }
            else if (subNode.type == FlowNodeTypesEnum.Branch)
            {
                // 分支节点
                return await ProcessBranchNode(dataDao, subNode, userId);
            }

            await SaveDataRemark(dataDao, $"无效的审批节点：node_id:{subNode.id},node_name:{subNode.nodeName},node_type:{subNode.type}！");
            return false;
        }

        /// <summary>
        /// 审批拒绝
        /// </summary>
        /// <param name="id">数据ID</param>
        /// <param name="comment">审批注释</param>
        /// <returns></returns>
        public async Task<bool> RejectAsync(long id, string comment, long userId)
        {
            var dataDao = await _SqlClient.Queryable<ScmFlowDataHeaderDao>().FirstAsync(a => a.id == id);
            if (dataDao == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 处理审批节点
        /// </summary>
        /// <param name="dataDao"></param>
        /// <param name="curNode"></param>
        /// <returns></returns>
        protected async Task<bool> ProcessApproveNode(ScmFlowDataHeaderDao dataDao, FlowNodeDto curNode, long userId)
        {
            var userList = GetUserList0(curNode.setType, curNode.nodeUserList);
            if (userList == null || userList.Count < 1)
            {
                return false;
            }

            curNode.examineMode = 1;
            // 更新或签人员审批状态
            var detailDaoList = await _SqlClient.Queryable<ScmFlowDataDetailDao>()
                .Where(a => a.header_id == dataDao.id && a.node_id == dataDao.id && a.node_id != userId)
                .ToListAsync();
            if (detailDaoList.Count > 0)
            {
                foreach (var detailDao in detailDaoList)
                {
                    detailDao.wfa_result = FlowDataResultEnum.Noneed;
                }
                await _SqlClient.Updateable(detailDaoList).ExecuteCommandAsync();
            }

            // 记录下一个节点
            dataDao.node_id = long.Parse(curNode.id);
            dataDao.remark = "等待审批";
            await _SqlClient.Updateable(dataDao).ExecuteCommandAsync();

            // 追加审批人员
            var detailList = new List<ScmFlowDataDetailDao>();
            foreach (var user in userList)
            {
                var detailDao = new ScmFlowDataDetailDao();
                detailDao.header_id = dataDao.id;
                detailDao.node_id = long.Parse(curNode.id);
                detailDao.user_id = user.id;
                detailDao.types = FlowDataDetailEnum.Primary;
                detailDao.wfa_result = FlowDataResultEnum.Todo;
                detailList.Add(detailDao);
            }
            await _SqlClient.Insertable(detailList).ExecuteCommandAsync();

            return true;
        }

        /// <summary>
        /// 处理分支节点
        /// </summary>
        /// <param name="dataDao"></param>
        /// <param name="subNode"></param>
        /// <returns></returns>
        protected async Task<bool> ProcessBranchNode(ScmFlowDataHeaderDao dataDao, FlowNodeDto subNode, long userId)
        {
            // 记录下一个节点
            dataDao.node_id = long.Parse(subNode.id);
            dataDao.remark = "审批结束";
            await _SqlClient.Updateable(dataDao).ExecuteCommandAsync();

            return true;
        }

        /// <summary>
        /// 处理结束节点
        /// </summary>
        /// <param name="dataDao"></param>
        /// <param name="subNode"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected async Task<bool> ProcessEndNode(ScmFlowDataHeaderDao dataDao, FlowNodeDto subNode, long userId)
        {
            await SaveDataResult(dataDao, "审批完成", userId);

            UpdateWfaStatus(dataDao.order_id, ScmWfaStatusEnum.Accept);
            return true;
        }

        /// <summary>
        /// 记录审批说明
        /// </summary>
        /// <param name="dataDao"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        protected async Task SaveDataRemark(ScmFlowDataHeaderDao dataDao, string remark)
        {
            LogUtils.Error(remark);

            dataDao.remark = remark;
            await _SqlClient.Updateable(dataDao).ExecuteCommandAsync();
        }

        /// <summary>
        /// 追加审批日志
        /// </summary>
        /// <param name="dataDao"></param>
        /// <param name="comment"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected async Task SaveDataResult(ScmFlowDataHeaderDao dataDao, string comment, long userId)
        {
            var resultDao = new ScmFlowDataResultDao();
            resultDao.data_id = dataDao.id;
            resultDao.flow_id = dataDao.flow_id;
            resultDao.result = FlowDataResultEnum.Accept;
            resultDao.comment = comment;
            resultDao.PrepareCreate(userId);
            await _SqlClient.Insertable(resultDao).ExecuteCommandAsync();
        }

        /// <summary>
        /// 判断是否满足条件
        /// </summary>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public virtual bool IsMath(List<WorkflowParams> paramList)
        {
            return false;
        }

        /// <summary>
        /// 获取符合条件的用户
        /// </summary>
        /// <returns></returns>
        public virtual List<NodeDataItem> GetUserList0(NodeAuthTypeEnum type, List<NodeDataItem> items)
        {
            if (type == NodeAuthTypeEnum.A)
            {
                return items;
            }
            if (type == NodeAuthTypeEnum.B)
            {
                var ids = items.Select(a => a.id).ToList();
                var list = _SqlClient.Queryable<UserRoleDao>()
                    .Where(a => ids.Contains(a.role_id) && a.row_status == ScmRowStatusEnum.Enabled)
                    .Select(a => new NodeDataItem { id = a.id, name = "" })
                    .ToList();
                return list;
            }

            return null;
        }
    }
}