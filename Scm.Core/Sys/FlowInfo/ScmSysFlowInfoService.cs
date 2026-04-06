using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Sys.FlowInfo.Dvo;
using Com.Scm.Sys.Workflow;
using Com.Scm.Utils;
using Com.Scm.Workflow.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.FlowInfo
{
    /// <summary>
    /// 工作流服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysFlowInfoService : ApiService
    {
        private readonly SugarRepository<FlowInfoDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmSysFlowInfoService(SugarRepository<FlowInfoDao> thisRepository, EnvConfig envConfig)
        {
            _thisRepository = thisRepository;
            _EnvConfig = envConfig;
        }

        /// <summary>
        /// 查询所有——分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<SysFlowInfoDto>> GetPageAsync(ScmSearchPageRequest param)
        {
            var query = await _thisRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(param.key), m => m.title.Contains(param.key))
                .Select<SysFlowInfoDto>()
                .ToPageAsync(param.page, param.limit);
            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ResOptionDvo>> OptionAsync()
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .Select(a => new ResOptionDvo { id = a.id, label = a.title, value = a.id })
                .ToListAsync();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SysFlowInfoDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<SysFlowInfoDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SysFlowInfoDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<SysFlowInfoDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SysFlowInfoDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<SysFlowInfoDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(SysFlowInfoDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.title == model.title);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的流程！");
            }

            dao = model.Adapt<FlowInfoDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(SysFlowInfoDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.title == model.title && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的流程！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return false;
            }

            dao = model.Adapt(dao);
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 删除,支持批量
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync([FromBody] List<long> ids) =>
            await _thisRepository.DeleteAsync(m => ids.Contains(m.id));

        /// <summary>
        /// 读取工作流配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<string> ReadFlowAsync(long id)
        {
            return await _EnvConfig.ReadFileAsync("flow", id + ".json");
            //if (string.IsNullOrEmpty(json))
            //{
            //    return null;
            //}
            //return json.AsJsonObject<WorkflowConfig>();
        }

        /// <summary>
        /// 保存工作流配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<bool> SaveFlowAsync(FlowInfoDto config)
        {
            if (config == null)
            {
                return false;
            }

            CheckNode(config.nodeConfig);

            var json = config.ToJsonString(false, true);
            await _EnvConfig.SaveFileAsync("flow", config.id + ".json", json);
            return true;
        }

        private void CheckNode(FlowNodeDto node)
        {
            if (node == null)
            {
                return;
            }

            if (node.id == null || !ScmUtils.IsValidId(node.id))
            {
                node.id = UidUtils.NextId().ToString();
            }

            CheckNode(node.childNode);
        }
    }
}
