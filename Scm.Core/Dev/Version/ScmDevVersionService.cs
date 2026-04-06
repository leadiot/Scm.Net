using Com.Scm.Dev.Version.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Service;
using Com.Scm.Utils;

using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Dev.Version
{
    /// <summary>
    /// 版本管理
    /// </summary>
    [ApiExplorerSettings(GroupName = "Dev")]
    public class ScmDevVersionService : ApiService
    {
        private readonly SugarRepository<ScmDevVerHeaderDao> _thisRepository;
        private readonly SugarRepository<ScmDevAppDao> _appRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmDevVersionService(SugarRepository<ScmDevVerHeaderDao> thisRepository, SugarRepository<ScmDevAppDao> appRepository)
        {
            _thisRepository = thisRepository;
            _appRepository = appRepository;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<VerHeaderDvo>> GetPagesAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(request.client != Enums.ScmClientTypeEnum.None, a => a.client == request.client)
                .OrderByDescending(m => m.id)
                .Select<VerHeaderDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<List<VerHeaderDvo>> GetListAsync(string code)
        {
            var appId = ScmDevAppDto.NET_ID;
            if (!string.IsNullOrWhiteSpace(code))
            {
                var appDao = await _appRepository.AsQueryable()
                    .Where(a => a.code == code)
                    .FirstAsync();
                if (appDao == null)
                {
                    return null;
                }

                appId = appDao.id;
            }

            var list = await _thisRepository.AsQueryable()
                .Where(a => a.app_id == appId && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .OrderByDescending(a => a.id)
                .Select<VerHeaderDvo>()
                .ToListAsync();

            Prepare(list);
            return list;
        }

        private void Prepare(List<VerHeaderDvo> list)
        {
            foreach (var item in list)
            {
                var dao = _appRepository.GetById(item.app_id);
                item.app_name = dao?.name;
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmDevVerHeaderDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmDevVerHeaderDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmDevVerHeaderDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmDevVerHeaderDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmDevVerHeaderDto> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmDevVerHeaderDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmDevVerHeaderDto model)
        {
            var versionDao = model.Adapt<ScmDevVerHeaderDao>();
            return await _thisRepository.InsertAsync(versionDao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ScmDevVerHeaderDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return;
            }

            dao = model.Adapt(dao);
            await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 设置当前版本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<bool> CurrentAsync(long id)
        {
            var dao = await _thisRepository.GetByIdAsync(id);

            var qty = await _thisRepository.AsUpdateable()
                .SetColumns(a => a.current == false)
                .Where(a => a.app_id == dao.app_id && a.client == dao.client)
                .ExecuteCommandAsync();

            dao.current = true;

            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 获取指定版本
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<VerHeaderDvo> GetVerAsync(GetVerRequest request)
        {
            var headerDto = await _thisRepository.AsQueryable()
                .Where(a => a.client == request.client && a.app_id == request.app_id && a.current == true)
                .Select<VerHeaderDvo>()
                .FirstAsync();

            if (headerDto != null)
            {
                var detailList = await _thisRepository.Change<ScmDevVerDetailDao>().AsQueryable()
                    .Where(a => a.ver_id == headerDto.id)
                    .Select<VerDetailDvo>()
                    .ToListAsync();
                headerDto.details = detailList;
            }
            return headerDto;
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