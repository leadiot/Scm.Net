using Com.Scm.Dsa;
using Com.Scm.Sys.Adv;
using Com.Scm.Sys.SysAdvColumn.Dto;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.SysAdvColumn;

/// <summary>
/// 服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysAdvColumnService : IApiService
{
    private readonly SugarRepository<ScmAdvColumnDao> _thisRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmSysAdvColumnService(SugarRepository<ScmAdvColumnDao> thisRepository)
    {
        _thisRepository = thisRepository;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<SysAdvColumnDto>> GetPagesAsync(ScmSearchPageRequest param)
    {
        return await _thisRepository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(param.key), m => m.Name.Contains(param.key))
            .Select<SysAdvColumnDto>()
            .ToPageAsync(param.page, param.limit);
    }

    /// <summary>
    /// 查询所有
    /// </summary>
    /// <returns></returns>
    public async Task<List<SysAdvColumnDto>> GetListAsync(ScmSearchPageRequest param)
    {
        return await _thisRepository.AsQueryable()
            .OrderBy(m => m.id, OrderByType.Desc)
            .Select<SysAdvColumnDto>()
            .ToListAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<SysAdvColumnDto> GetAsync(long id)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.id == id)
            .Select<SysAdvColumnDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(SysAdvColumnDto model)
    {
        if (model.ParentIdList.All(m => m != "0"))
        {
            model.ParentId = long.Parse(model.ParentIdList.Last());
            var paramModel = await _thisRepository.GetByIdAsync(model.ParentId);
            model.Layer = paramModel.Layer + 1;
            model.ParentIdList.Add(model.id.ToString());
        }
        else
        {
            model.ParentIdList = new List<string> { model.id.ToString() };
        }

        var upModel = await _thisRepository.GetFirstAsync(m => true, m => m.Sort);
        model.Sort = upModel.Sort + 1;
        return await _thisRepository.InsertAsync(model.Adapt<ScmAdvColumnDao>());
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(SysAdvColumnDto model)
    {
        if (model.ParentIdList.All(m => m != "0"))
        {
            model.ParentId = long.Parse(model.ParentIdList.Last());
            model.ParentIdList.Add(model.id.ToString());
        }
        else
        {
            model.ParentIdList = new List<string> { model.id.ToString() };
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
    /// 删除,支持多个
    /// </summary>
    /// <param name="ids">逗号分隔</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(string ids)
    {
        return await _thisRepository.DeleteAsync(m => ids.ToListLong().Contains(m.id));
    }
}

