using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Ur.Group.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Ur.Group;

/// <summary>
/// 服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Ur")]
public class ScmUrGroupService : ApiService
{
    private readonly SugarRepository<GroupDao> _thisRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmUrGroupService(SugarRepository<GroupDao> thisRepository, IUserHolder userService)
    {
        _thisRepository = thisRepository;
        _UserHolder = userService;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<GroupDvo>> GetPagesAsync(SearchRequest request)
    {
        var result = await _thisRepository.AsQueryable()
            .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
            .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
            .Select<GroupDvo>()
            .ToPageAsync(request.page, request.limit);

        Prepare(result.Items);
        return result;
    }

    /// <summary>
    /// 查询所有
    /// </summary>
    /// <returns></returns>
    public async Task<List<GroupDvo>> GetListAsync(SearchRequest request)
    {
        var result = await _thisRepository.AsQueryable()
            .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
            .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
            .Select<GroupDvo>()
            .ToListAsync();

        Prepare(result);
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<List<ResOptionDvo>> OptionAsync(long id)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
            .Select(a => new ResOptionDvo { id = a.id, label = a.names, value = a.id, parentId = a.pid })
            .ToListAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<GroupDto> GetAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<GroupDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<GroupDto> GetEditAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<GroupDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<GroupDvo> GetViewAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<GroupDvo>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(GroupDto model)
    {
        var organizeDao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
        if (organizeDao != null)
        {
            throw new BusinessException($"已存在编码为{model.codec}的群组！");
        }
        organizeDao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
        if (organizeDao != null)
        {
            throw new BusinessException($"已存在名称为{model.codec}的群组！");
        }

        organizeDao = model.Adapt<GroupDao>();
        await _thisRepository.InsertAsync(organizeDao);

        return organizeDao.id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(GroupDto model)
    {
        var organizeDao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
        if (organizeDao != null)
        {
            throw new BusinessException($"已存在编码为{model.codec}的群组！");
        }
        organizeDao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec && a.id != model.id);
        if (organizeDao != null)
        {
            throw new BusinessException($"已存在名称为{model.codec}的群组！");
        }

        organizeDao = await _thisRepository.GetByIdAsync(model.id);
        if (organizeDao == null)
        {
            throw new BusinessException("无效的群组信息！");
        }

        organizeDao = model.Adapt(organizeDao);
        return await _thisRepository.UpdateAsync(organizeDao);
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

