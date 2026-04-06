using Com.Scm.Cfg.Export;
using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Ur.User;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Cfg.ExportHeader
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmCfgExportHeaderService : ApiService
    {
        private readonly SugarRepository<ExportHeaderDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmCfgExportHeaderService(SugarRepository<ExportHeaderDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ExportHeaderDto>> GetPagesAsync(ScmSearchPageRequest param)
        {
            return await _thisRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(param.key), m => m.names.Contains(param.key))
                .Select<ExportHeaderDto>()
                .ToPageAsync(param.page, param.limit);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<ExportHeaderDto>> GetListAsync(ScmSearchPageRequest param)
        {
            return await _thisRepository.AsQueryable()
                .OrderBy(m => m.id, OrderByType.Desc)
                .Select<ExportHeaderDto>()
                .ToListAsync();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ExportHeaderDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<ExportHeaderDto>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ExportHeaderDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ExportHeaderDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ExportHeaderDto> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ExportHeaderDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ExportHeaderDto model)
        {
            var isAny = await _thisRepository.IsAnyAsync(m => m.codec == model.codec);
            if (isAny)
            {
                throw new BusinessException("标识不能重复~");
            }

            return await _thisRepository.InsertAsync(model.Adapt<ExportHeaderDao>());
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ExportHeaderDto model)
        {
            var isAny = await _thisRepository.IsAnyAsync(m => m.codec == model.codec && m.id != model.id);
            if (isAny)
            {
                throw new BusinessException("标识不能重复~");
            }
            return await _thisRepository.UpdateAsync(model.Adapt<ExportHeaderDao>());
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetResetAsync()
        {
            var dao = new UserExportHandler().GenDao();

            var qty = await _thisRepository.InsertAsync(dao);

            await _thisRepository.Change<ExportDetailDao>().InsertRangeAsync(dao.details);

            return 0;
        }
    }
}
