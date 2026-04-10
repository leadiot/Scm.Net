using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Sys.Config.Dvo;
using Com.Scm.Token;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Config
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysConfigService : ApiService
    {
        private readonly SugarRepository<ConfigDao> _thisRepository;
        private readonly ScmContextHolder _jwtHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="jwtHolder"></param>
        public ScmSysConfigService(SugarRepository<ConfigDao> thisRepository, ScmContextHolder jwtHolder)
        {
            _thisRepository = thisRepository;
            _jwtHolder = jwtHolder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ScmSearchPageResponse<ConfigDto>> GetPagesAsync(SearchRequest request)
        {
            var user = _jwtHolder.GetToken();

            return await _thisRepository
                .AsQueryable()
                .Where(a => (a.user_id == user.user_id || a.user_id == UserDto.SYS_ID) && a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(request.client != ScmClientTypeEnum.None, a => a.client == request.client)
                .WhereIF(IsNormalId(request.id), a => a.cat_id == request.id)
                .Select<ConfigDto>()
                .ToPageAsync(request.page, request.limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ConfigDto>> GetListAsync(SearchRequest request)
        {
            var user = _jwtHolder.GetToken();

            return await _thisRepository
                .AsQueryable()
                .Where(a => (a.user_id == user.user_id || a.user_id == UserDto.SYS_ID) && a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(request.client != ScmClientTypeEnum.None, a => a.client == request.client)
                .Select<ConfigDto>()
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public async Task<ConfigDto> GetKeyAsync(string key)
        {
            var token = _jwtHolder.GetToken();
            var userId = token.user_id;

            return await _thisRepository.AsQueryable()
                .Where(a => a.key == key && a.row_status == ScmRowStatusEnum.Enabled)
                .Where(a => a.user_id == userId || a.user_id == UserDto.SYS_ID)
                .OrderBy(a => a.user_id, SqlSugar.OrderByType.Desc)
                .Select<ConfigDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 批量保存
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="client"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<bool> SaveConfigAsync(long userId, ScmClientTypeEnum client, string key, string value, ScmDataTypeEnum data)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.user_id == userId && a.key == key);
            if (dao == null)
            {
                dao = new ConfigDao();
                dao.user_id = userId;
                dao.client = client;
                dao.key = key;
                dao.value = value ?? "";
                dao.data = data;
                return await _thisRepository.InsertAsync(dao);
            }

            dao.client = client;
            dao.key = key;
            dao.value = value ?? "";
            dao.data = data;
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> BatchAsync(List<ConfigDto> items)
        {
            var user = _jwtHolder.GetToken();

            foreach (var item in items)
            {
                await SaveConfigAsync(user.user_id, item.client, item.key, item.value, item.data);
            }

            return true;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ConfigDto model)
        {
            var isAny = await _thisRepository.IsAnyAsync(a => a.key == model.key && a.cat_id == model.cat_id);
            if (isAny)
            {
                throw new BusinessException("标识不能重复~");
            }

            if (!IsValidId(model.user_id))
            {
                model.user_id = UserDto.SYS_ID;
            }

            var dao = model.Adapt<ConfigDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ConfigDto> GetAsync(long id)
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.id == id)
                .Select<ConfigDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ConfigDto model)
        {
            var isAny = await _thisRepository.IsAnyAsync(a => a.key == model.key && a.cat_id == model.cat_id && a.id != model.id);
            if (isAny)
            {
                throw new BusinessException("标识不能重复~");
            }

            if (!IsValidId(model.user_id))
            {
                model.user_id = UserDto.SYS_ID;
            }

            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return false;
            }

            dao = model.Adapt(dao);
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 更新记录状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus(_thisRepository, param.ids, param.status);
        }

        /// <summary>
        /// 删除记录,支持多个
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
