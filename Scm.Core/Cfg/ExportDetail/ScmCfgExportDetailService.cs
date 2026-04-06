using Com.Scm.Cfg.Export;
using Com.Scm.Cfg.ExportDetail.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Cfg.ExportDetail
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmCfgExportDetailService : ApiService
    {
        private readonly SugarRepository<ExportDetailDao> _thisRepository;
        private SugarRepository<ExportHeaderDao> _headerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="headerRepository"></param>
        public ScmCfgExportDetailService(SugarRepository<ExportDetailDao> thisRepository, SugarRepository<ExportHeaderDao> headerRepository)
        {
            _thisRepository = thisRepository;
            _headerRepository = headerRepository;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ExportDetailDto>> GetPagesAsync(SearchRequest request)
        {
            var hid = request.hid;
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
                .WhereIF(IsNormalId(request.hid), m => m.export_id == hid)
                .Select<ExportDetailDto>()
                .ToPageAsync(request.page, request.limit);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ExportDetailDto>> GetListAsync(SearchRequest request)
        {
            if (!string.IsNullOrEmpty(request.codec))
            {
                var typeModel = await _headerRepository.AsQueryable()
                    .FirstAsync(m => m.codec == request.codec);
                if (typeModel != null)
                {
                    request.id = typeModel.id;
                }
            }
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
                .WhereIF(IsNormalId(request.id), m => m.export_id == request.id)
                .Select<ExportDetailDto>()
                .ToListAsync();
            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ExportDetailDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<ExportDetailDto>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ExportDetailDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ExportDetailDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ExportDetailDto> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ExportDetailDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddAsync(ExportDetailDto model)
        {
            var isAny = await _thisRepository.IsAnyAsync(m => m.export_id == model.export_id && m.namec == model.namec);
            if (isAny)
            {
                throw new BusinessException("名称不能重复~");
            }
            await _thisRepository.InsertReturnSnowflakeIdAsync(model.Adapt<ExportDetailDao>());
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ExportDetailDto model)
        {
            var list = await _thisRepository.GetListAsync(m => m.export_id == model.export_id);
            if (list != null && list.Count > 0)
            {
                //var tmpDao = list.Find(a => a.value == model.value && a.id != model.id);
                //if (tmpDao != null)
                //{
                //    throw new BusinessException($"已存在值为{model.value}的子项！");
                //}
                //tmpDao = list.Find(a => a.codec == model.codec && a.id != model.id);
                //if (tmpDao != null)
                //{
                //    throw new BusinessException($"已存在代码为{model.codec}的子项！");
                //}
                //tmpDao = list.Find(a => a.namec == model.namec && a.id != model.id);
                //if (tmpDao != null)
                //{
                //    throw new BusinessException($"已存在名称为{model.namec}的子项！");
                //}
            }

            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException("无效的子项信息！");
            }
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
