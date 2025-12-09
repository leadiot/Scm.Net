using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Jwt;
using Com.Scm.Samples.PoHeader.Dao;
using Com.Scm.Samples.PoHeader.Dto;
using Com.Scm.Service;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Samples.PoHeader
{
    [ApiExplorerSettings(GroupName = "Samples")]
    public class SamplesPoFlowService : ApiFlowService
    {
        public SamplesPoFlowService(ISqlSugarClient sqlClient, EnvConfig envConfig, JwtContextHolder jwtHolder)
        {
            _SqlClient = sqlClient;
            _EnvConfig = envConfig;
            _JwtHolder = jwtHolder;
        }

        /// <summary>
        /// 提交审批
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<bool> SubmitAsync(long id)
        {
            var dao = await _SqlClient.Queryable<SamplesPoHeaderDao>().FirstAsync(a => a.id == id);
            if (dao == null)
            {
                throw new BusinessException("无效的采购单！");
            }

            // 查询流程
            var flowOrderDao = await GetFlowOrderAsync(SamplesPoHeaderDto.FLOW_CODE);
            if (flowOrderDao == null)
            {
                return false;
            }

            // 更新审批状态
            dao.wfa_status = ScmWfaStatusEnum.Doing;
            await _SqlClient.Updateable(dao).ExecuteCommandAsync();

            // 启动流程
            var orderId = dao.id;
            var orderCode = dao.codes;
            return await StartFlow(flowOrderDao, orderId, orderCode, dao.update_user);
        }

        /// <summary>
        /// 更新单据状态
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        protected override async Task ChangeOrderWfaStatus(long orderId, ScmWfaStatusEnum status)
        {
            var dao = await _SqlClient.Queryable<SamplesPoHeaderDao>().FirstAsync(a => a.id == orderId);
            if (dao == null)
            {
                return;
            }

            dao.wfa_status = status;
            await _SqlClient.Updateable(dao).ExecuteCommandAsync();
        }
    }
}
