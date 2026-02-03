using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Exceptions;
using Com.Scm.Res.App;
using Com.Scm.Res.Ext.Dvo;
using Com.Scm.Res.Org;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Res.Ext
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Scm")]
    public class ScmResExtService : ApiService
    {
        private readonly SugarRepository<ScmResExtDao> _thisRepository;
        private readonly IDicService _dicService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmResExtService(SugarRepository<ScmResExtDao> thisRepository, IResHolder resHolder, IDicService dicService)
        {
            _thisRepository = thisRepository;
            _ResHolder = resHolder;
            _dicService = dicService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ScmResExtDvo>> GetPagesAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(IsNormalId(request.org_id), a => a.org_id == request.org_id)
                .WhereIF(IsNormalId(request.app_id), a => a.app_id == request.app_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.codec.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmResExtDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmResExtDvo>> GetListAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(IsNormalId(request.org_id), a => a.org_id == request.org_id)
                .WhereIF(IsNormalId(request.app_id), a => a.app_id == request.app_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.codec.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmResExtDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        private void Prepare(List<ScmResExtDvo> list)
        {
            var dicDao = _dicService.GetDic("file_type");
            var orgRepository = _thisRepository.Change<ScmResOrgDao>();
            var orgDict = new Dictionary<long, ScmResOrgDao>();
            var appRepository = _thisRepository.Change<ScmResAppDao>();
            var appDict = new Dictionary<long, ScmResAppDao>();
            foreach (var item in list)
            {
                Prepare(item);

                item.kind_name = dicDao.GetDetail((int)item.kind)?.namec;

                ScmResOrgDao orgDao = null;
                if (orgDict.ContainsKey(item.org_id))
                {
                    orgDao = orgDict[item.org_id];
                }
                else
                {
                    orgDao = orgRepository.GetById(item.org_id);
                    orgDict[item.org_id] = orgDao;
                }
                item.org_name = orgDao?.names;

                ScmResAppDao appDao = null;
                if (appDict.ContainsKey(item.app_id))
                {
                    appDao = appDict[item.app_id];
                }
                else
                {
                    appDao = appRepository.GetById(item.app_id);
                    appDict[item.app_id] = appDao;
                }
                item.app_name = appDao?.names;
            }
        }

        /// <summary>
        /// 下拉列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ResOptionDvo>> GetOptionAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(IsNormalId(request.org_id), a => a.org_id == request.org_id)
                .WhereIF(IsNormalId(request.app_id), a => a.app_id == request.app_id)
                .OrderBy(a => a.id)
                .Select(a => new ResOptionDvo { id = a.id, label = a.namec, value = a.id })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmResExtDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmResExtDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmResExtDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmResExtDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmResExtDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmResExtDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmResExtDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的后缀！");
            }

            dao = model.Adapt<ScmResExtDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ScmResExtDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的后缀！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException("无效的后缀！");
            }

            dao = model.Adapt(dao);
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus(_thisRepository, param.ids, param.status);
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> DeleteAsync(string ids)
        {
            return await DeleteRecord(_thisRepository, ids.ToListLong());
        }
    }
}