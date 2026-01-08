using Com.Scm.Dev.Uid.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Utils;

using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Dev.Uid
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Dev")]
    public class ScmDevUidService : ApiService
    {
        private readonly SugarRepository<ScmDevUidDao> _thisRepository;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="userRepository"></param>
        /// <returns></returns>
        public ScmDevUidService(SugarRepository<ScmDevUidDao> thisRepository, IUserHolder userService)
        {
            _thisRepository = thisRepository;
            _ResHolder = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ScmUidDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmUidDvo>()
                .ToPageAsync(request.page, request.limit);

            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmUidDvo>> GetListAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                //.Where(a => a.row_status == Com.Scm.Enums.ScmStatusEnum.Enabled)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmUidDvo>()
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmDevUidDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmDevUidDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmDevUidDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmDevUidDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmUidDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmUidDvo>()
                .FirstAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmDevUidDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.k == model.k);
            if (dao != null)
            {
                throw new BusinessException($"已存在编码为{model.k}的！");
            }

            return await _thisRepository.InsertAsync(model.Adapt<ScmDevUidDao>());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ScmDevUidDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.k == model.k && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException($"已存在编码为{model.k}的！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException($"无效的数据信息，更新失败！");
            }

            dao = model.Adapt(dao);
            await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(string ids)
        {
            var list = ids.ToListLong();
            return await _thisRepository.DeleteAsync(a => list.Contains(a.id));
        }
    }
}