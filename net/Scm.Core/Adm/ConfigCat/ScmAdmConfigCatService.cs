using Com.Scm.Adm.Config;
using Com.Scm.Adm.ConfigCat.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Adm.ConfigCat
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmAdmConfigCatService : ApiService
    {
        private readonly SugarRepository<AdmConfigCatDao> _thisRepository;
        private readonly ScmContextHolder _jwtHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="jwtHolder"></param>
        public ScmAdmConfigCatService(SugarRepository<AdmConfigCatDao> thisRepository, ScmContextHolder jwtHolder)
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
        public async Task<List<AdmConfigCatDto>> GetListAsync(SearchRequest request)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.ToString().Contains(request.key))
                .Select<AdmConfigCatDto>()
                .ToListAsync();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<AdmConfigCatDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<AdmConfigCatDto>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AdmConfigCatDto model)
        {
            var isAny = await _thisRepository.IsAnyAsync(m => m.codec == model.codec);
            if (isAny)
            {
                throw new BusinessException("标识不能重复~");
            }

            var dao = await _thisRepository.GetFirstAsync(m => true, m => m.od);
            model.od = dao.od + 1;
            dao = model.Adapt<AdmConfigCatDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(AdmConfigCatDto model)
        {
            var isAny = await _thisRepository.IsAnyAsync(m => m.codec == model.codec && m.id != model.id);
            if (isAny)
            {
                throw new BusinessException("标识不能重复~");
            }

            var dao = await _thisRepository.GetByIdAsync(model.id);
            dao = CommonUtils.Adapt(model, dao);
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
