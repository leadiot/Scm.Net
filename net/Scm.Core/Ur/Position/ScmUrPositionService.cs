using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Ur.Position.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Ur.Position;

/// <summary>
/// 岗位表服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Ur")]
public class ScmUrPositionService : ApiService
{
    private readonly SugarRepository<PositionDao> _thisRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    /// <param name="userService"></param>
    public ScmUrPositionService(SugarRepository<PositionDao> thisRepository, IUserHolder userService)
    {
        _thisRepository = thisRepository;
        _UserService = userService;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<PositionDvo>> GetPagesAsync(ScmSearchPageRequest request)
    {
        var result = await _thisRepository
            .AsQueryable()
            .WhereIF(request.row_status != Enums.ScmRowStatusEnum.Normal, a => a.row_status == request.row_status)
            .WhereIF(!string.IsNullOrEmpty(request.key), a => a.namec.Contains(request.key))
            .OrderBy(a => a.id)
            .Select<PositionDvo>()
            .ToPageAsync(request.page, request.limit);

        Prepare(result.Items);
        return result;
    }

    /// <summary>
    /// 查询所有—
    /// </summary>
    /// <returns></returns>
    public async Task<List<PositionDto>> GetAllAsync()
    {
        return await _thisRepository
            .AsQueryable()
            .OrderBy(m => m.id, OrderByType.Desc)
            .Select<PositionDto>()
            .ToListAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<List<ResOptionDvo>> OptionAsync(long id)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
            .Select(a => new ResOptionDvo { id = a.id, label = a.namec, value = a.id })
            .ToListAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<PositionDto> GetAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<PositionDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(PositionDto model)
    {
        var dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
        if (dao != null)
        {
            throw new BusinessException("已存在相同的岗位名称：" + model.namec);
        }

        dao = model.Adapt<PositionDao>();
        return await _thisRepository.InsertAsync(dao);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(PositionDto model)
    {
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
