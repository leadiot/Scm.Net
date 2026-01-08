using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Sys.Uom.Dto;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysUomService : ApiService, IUomService
    {
        private readonly SugarRepository<ScmSysUomDao> _thisRepository;
        private static Dictionary<long, ScmSysUomDao> _Dict = new Dictionary<long, ScmSysUomDao>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmSysUomService(SugarRepository<ScmSysUomDao> thisRepository, IUserHolder userService)
        {
            _thisRepository = thisRepository;
            _ResHolder = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ScmSysUomDvo>> GetPagesAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(request.types != Enums.ScmUomTypesEnum.None, a => a.types == request.types)
                .WhereIF(request.modes != Enums.ScmUomModesEnum.None, a => a.modes == request.modes)
                .WhereIF(request.kinds != Enums.ScmUomKindsEnum.None, a => a.kinds == request.kinds)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.namec.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmSysUomDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmSysUomDvo>> GetListAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Com.Scm.Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(request.types != Enums.ScmUomTypesEnum.None, a => a.types == request.types)
                .WhereIF(request.modes != Enums.ScmUomModesEnum.None, a => a.modes == request.modes)
                .WhereIF(request.kinds != Enums.ScmUomKindsEnum.None, a => a.kinds == request.kinds)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.namec.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmSysUomDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        private void Prepare(List<ScmSysUomDvo> items)
        {
            foreach (var item in items)
            {
                var referDao = _thisRepository.GetById(item.refer_id);
                item.refer_names = referDao?.names;

                var basicDao = _thisRepository.GetById(item.basic_id);
                item.basic_names = basicDao?.names;
            }
        }

        /// <summary>
        /// 下拉列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ResOptionDvo>> GetOptionAsync(OptionRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled && a.lang == request.lang)
                .WhereIF(request.types != Enums.ScmUomTypesEnum.None, a => a.types == request.types)
                .WhereIF(request.modes != Enums.ScmUomModesEnum.None, a => a.modes == request.modes)
                .WhereIF(request.kinds != Enums.ScmUomKindsEnum.None, a => a.kinds == request.kinds)
                .OrderBy(a => a.od)
                .Select(a => new ResOptionDvo { id = a.id, label = a.names, value = a.id })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmSysUomDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmSysUomDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmSysUomDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmSysUomDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmSysUomDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmSysUomDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmSysUomDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的计量单位！");
            }
            dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的计量单位！");
            }

            dao = model.Adapt<ScmSysUomDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ScmSysUomDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的计量单位！");
            }
            dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的计量单位！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException("无效的计量单位！");
            }

            dao = model.Adapt(dao);
            RemoveById(model.id);
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 更新参照数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> ChangeReferAsync(ChangeReferRequest request)
        {
            var referDao = await _thisRepository.GetFirstAsync(a => a.id == request.refer_id && a.row_status == Enums.ScmRowStatusEnum.Enabled);
            if (referDao == null)
            {
                throw new BusinessException("无效的参照单位！");
            }
            var dao = await _thisRepository.GetFirstAsync(a => a.id == request.id);
            if (dao == null)
            {
                throw new BusinessException("无效的计量单位！");
            }

            dao.refer_id = request.refer_id;
            dao.refer_qty = request.refer_qty;
            RemoveById(request.id);
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 更新基准数量
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<bool> ChangeBasicAsync(ChangeBasicRequest request)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.id == request.basic_id);
            if (dao == null)
            {
                throw new BusinessException("无效的参照单位！");
            }
            dao = await _thisRepository.GetFirstAsync(a => a.id == request.id);
            if (dao == null)
            {
                throw new BusinessException("无效的计量单位！");
            }

            dao.basic_id = request.basic_id;
            dao.basic_qty = request.basic_qty;
            RemoveById(request.id);
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

        public ScmSysUomDao GetUom(long id)
        {
            if (_Dict.ContainsKey(id))
            {
                return _Dict[id];
            }

            var dao = _thisRepository.GetById(id);
            _Dict[id] = dao;
            return dao;
        }

        public void RemoveById(long id)
        {
            if (_Dict.ContainsKey(id))
            {
                _Dict.Remove(id);
            }
        }
    }
}