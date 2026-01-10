using Com.Scm.Dsa;
using Com.Scm.Sys.Adv;
using Com.Scm.Sys.DicDetail.Dvo;
using Com.Scm.Sys.SysAdvInfo.Dto;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.SysAdvInfo;

/// <summary>
/// 广告位信息表 服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysAdvInfoService : IApiService
{
    private readonly SugarRepository<ScmAdvInfoDao> _thisRepository;
    private readonly SugarRepository<ScmAdvColumnDao> _advColumnRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    /// <param name="advColumnRepository"></param>
    public ScmSysAdvInfoService(SugarRepository<ScmAdvInfoDao> thisRepository
    , SugarRepository<ScmAdvColumnDao> advColumnRepository)
    {
        _thisRepository = thisRepository;
        _advColumnRepository = advColumnRepository;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<SysAdvInfoDto>> GetPagesAsync(SearchRequest param)
    {
        if (!string.IsNullOrEmpty(param.codec))
        {
            var typeModel = await _advColumnRepository.GetSingleAsync(m => m.Flag == param.codec);
            if (typeModel != null)
            {
                param.id = typeModel.id;
            }
        }
        var query = await _thisRepository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(param.key), m => m.Title.Contains(param.key))
            .WhereIF(param.id != 0, m => m.ColumnId == param.id)
            .OrderBy(m => new { m.id, m.Sort }, OrderByType.Desc)
            .Select<SysAdvInfoDto>()
            .ToPageAsync(param.page, param.limit);
        return query;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<SysAdvInfoDto> GetAsync(long id)
    {
        var model = await _thisRepository.GetByIdAsync(id);
        return model.Adapt<SysAdvInfoDto>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(SysAdvInfoDto model)
    {
        return await _thisRepository.InsertAsync(model.Adapt<ScmAdvInfoDao>());
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(SysAdvInfoDto model)
    {
        return await _thisRepository.UpdateAsync(model.Adapt<ScmAdvInfoDao>());
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
