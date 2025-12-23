using Com.Scm.Adm.Config.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Adm.Config
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Adm")]
    public class ScmAdmConfigService : ApiService
    {
        private readonly SugarRepository<AdmConfigDao> _thisRepository;
        private readonly ScmContextHolder _jwtHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="jwtHolder"></param>
        public ScmAdmConfigService(SugarRepository<AdmConfigDao> thisRepository, ScmContextHolder jwtHolder)
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
        public async Task<ScmSearchPageResponse<AdmConfigDto>> GetPagesAsync(SearchRequest request)
        {
            var user = _jwtHolder.GetToken();

            return await _thisRepository
                .AsQueryable()
                .Where(a => (a.user_id == user.user_id || a.user_id == UserDto.SYS_ID) && a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(request.client != ScmClientTypeEnum.None, a => a.client == request.client)
                .WhereIF(IsNormalId(request.id), a => a.cat_id == request.id)
                .Select<AdmConfigDto>()
                .ToPageAsync(request.page, request.limit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<AdmConfigDto>> GetListAsync(SearchRequest request)
        {
            var user = _jwtHolder.GetToken();

            return await _thisRepository
                .AsQueryable()
                .Where(a => (a.user_id == user.user_id || a.user_id == UserDto.SYS_ID) && a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(request.client != ScmClientTypeEnum.None, a => a.client == request.client)
                .Select<AdmConfigDto>()
                .ToListAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AdmConfigDto model)
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

            var dao = model.Adapt<AdmConfigDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<AdmConfigDto> GetAsync(long id)
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.id == id)
                .Select<AdmConfigDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(AdmConfigDto model)
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
