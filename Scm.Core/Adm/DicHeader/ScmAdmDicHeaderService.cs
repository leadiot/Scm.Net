using Com.Scm.Adm.Dic;
using Com.Scm.Adm.DicHeader.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Adm.DicHeader;

/// <summary>
/// 服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Adm")]
public class ScmAdmDicHeaderService : ApiService
{
    private readonly SugarRepository<AdmDicHeaderDao> _thisRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmAdmDicHeaderService(SugarRepository<AdmDicHeaderDao> thisRepository)
    {
        _thisRepository = thisRepository;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<AdmDicHeaderDto>> GetPagesAsync(SearchReuqest request)
    {
        var query = await _thisRepository.AsQueryable()
            .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
            .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
            .Select<AdmDicHeaderDto>()
            .ToPageAsync(request.page, request.limit);
        return query;
    }

    /// <summary>
    /// 查询所有
    /// </summary>
    /// <returns></returns>
    public async Task<List<AdmDicHeaderDto>> GetListAsync(SearchReuqest request)
    {
        var list = await _thisRepository.AsQueryable()
            .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
            .WhereIF(request.type != 0, m => m.types == request.type)
            .OrderBy(m => m.id, OrderByType.Desc)
            .Select<AdmDicHeaderDto>()
            .ToListAsync();
        return list;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<AdmDicHeaderDto> GetAsync(long id)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.id == id)
            .Select<AdmDicHeaderDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(AdmDicHeaderDto model)
    {
        var isAny = await _thisRepository.IsAnyAsync(m => m.types == model.types && m.codec == model.codec);
        if (isAny)
        {
            throw new BusinessException("标识不能重复~");
        }

        var upModel = await _thisRepository.GetFirstAsync(m => true, m => m.od);
        model.od = upModel.od + 1;
        return await _thisRepository.InsertAsync(model.Adapt<AdmDicHeaderDao>());
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(AdmDicHeaderDto model)
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

