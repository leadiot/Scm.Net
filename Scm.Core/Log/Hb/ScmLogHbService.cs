using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Filters;
using Com.Scm.Log.Hb.Dvo;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Log.Hb
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Log"), NoAuditLog]
    public class ScmLogHbService : IApiService
    {
        private readonly SugarRepository<LogHbDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="userRepository"></param>
        /// <returns></returns>
        public ScmLogHbService(SugarRepository<LogHbDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<LogHbDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<LogHbDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<LogHbDvo>> GetListAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<LogHbDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void Prepare(List<LogHbDvo> list)
        {
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserOAuthDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<UserOAuthDto>();
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<LogHbDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<LogHbDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(LogHbDto model)
        {
            return await _thisRepository.InsertAsync(model.Adapt<LogHbDao>());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(LogHbDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException($"无效的数据信息，更新失败！");
            }
            dao = model.Adapt(dao);
            await _thisRepository.UpdateAsync(dao);
        }
    }
}
