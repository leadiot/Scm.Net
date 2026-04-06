using Com.Scm.Dsa;
using Com.Scm.Service;
using Com.Scm.Sys.FlowNode.Dvo;
using Com.Scm.Sys.Workflow;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.FlowNode
{
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysFlowNodeService : ApiService
    {
        private readonly SugarRepository<FlowNodeDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmSysFlowNodeService(SugarRepository<FlowNodeDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 查询所有——分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<SysFlowNodeDto>> GetPagesAsync(SearchRequest param)
        {
            var query = await _thisRepository.AsQueryable()
                //.WhereIF(!string.IsNullOrEmpty(param.key), m => m.title.Contains(param.key))
                .Select<SysFlowNodeDto>()
                .ToPageAsync(param.page, param.limit);
            return query;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<List<SysFlowNodeDto>> GetListAsync(SearchRequest param)
        {
            var query = await _thisRepository.AsQueryable()
                .Where(a => a.flow_id == param.id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .OrderBy(a => a.od)
                .Select<SysFlowNodeDto>()
                .ToListAsync();
            return query;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SysFlowNodeDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<SysFlowNodeDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SysFlowNodeDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<SysFlowNodeDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<SysFlowNodeDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<SysFlowNodeDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(SysFlowNodeDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.title == model.title);
            //if (dao != null)
            //{
            //    throw new BusinessException("已存在相同名称的流程！");
            //}

            var dao = model.Adapt<FlowNodeDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(SysFlowNodeDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.title == model.title && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException("已存在相同名称的流程！");
            //}

            var dao = await _thisRepository.GetByIdAsync(model.id);
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
    }
}
