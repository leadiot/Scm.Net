using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Sys.Dic;
using Com.Scm.Sys.DicHeader.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.DicHeader;

/// <summary>
/// 服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysDicHeaderService : ApiService
{
    private readonly SugarRepository<DicHeaderDao> _thisRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmSysDicHeaderService(SugarRepository<DicHeaderDao> thisRepository)
    {
        _thisRepository = thisRepository;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<DicHeaderDto>> GetPagesAsync(SearchReuqest request)
    {
        var query = await _thisRepository.AsQueryable()
            .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
            .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
            .Select<DicHeaderDto>()
            .ToPageAsync(request.page, request.limit);
        return query;
    }

    /// <summary>
    /// 查询所有
    /// </summary>
    /// <returns></returns>
    public async Task<List<DicHeaderDto>> GetListAsync(SearchReuqest request)
    {
        var list = await _thisRepository.AsQueryable()
            .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
            .WhereIF(request.type != 0, m => m.types == request.type)
            .OrderBy(m => m.id, OrderByType.Desc)
            .Select<DicHeaderDto>()
            .ToListAsync();
        return list;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<DicHeaderDto> GetAsync(long id)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.id == id)
            .Select<DicHeaderDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(DicHeaderDto model)
    {
        var isAny = await _thisRepository.IsAnyAsync(m => m.types == model.types && m.codec == model.codec);
        if (isAny)
        {
            throw new BusinessException("标识不能重复~");
        }

        var upModel = await _thisRepository.GetFirstAsync(m => true, m => m.od);
        model.od = upModel.od + 1;
        return await _thisRepository.InsertAsync(model.Adapt<DicHeaderDao>());
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(DicHeaderDto model)
    {
        var isAny = await _thisRepository.IsAnyAsync(m => m.types == model.types && m.codec == model.codec && m.id != model.id);
        if (isAny)
        {
            throw new BusinessException("标识不能重复~");
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

