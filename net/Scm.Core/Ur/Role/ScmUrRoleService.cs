using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Ur.Role.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Ur.Role;

/// <summary>
/// 服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Ur")]
public class ScmUrRoleService : ApiService
{
    private readonly SugarRepository<RoleDao> _thisRepository;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmUrRoleService(SugarRepository<RoleDao> thisRepository, IResHolder resHolder)
    {
        _thisRepository = thisRepository;
        _ResHolder = resHolder;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<RoleDvo>> GetPagesAsync(SearchRequest request)
    {
        var result = await _thisRepository.AsQueryable()
            .WhereIF(request.IsAllStatus(), a => a.row_status == request.row_status)
            .Select<RoleDvo>()
            .ToPageAsync(request.limit, request.page);

        Prepare(result.Items);
        return result;
    }

    /// <summary>
    /// 查询所有
    /// </summary>
    /// <returns></returns>
    public async Task<List<RoleDvo>> GetListAsync(SearchRequest request)
    {
        var result = await _thisRepository.AsQueryable()
            .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
            .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
            .Select<RoleDvo>()
            .ToListAsync();

        Prepare(result);
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<List<ResOptionDvo>> GetOptionAsync()
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
            .Select(a => new ResOptionDvo { id = a.id, label = a.namec, value = a.id, parentId = a.pid })
            .ToListAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<RoleDto> GetAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<RoleDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<RoleDto> GetEditAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<RoleDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<RoleDvo> GetViewAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<RoleDvo>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(RoleDto model)
    {
        var roleDao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
        if (roleDao != null)
        {
            throw new BusinessException($"已存在名称为{model.namec}的角色！");
        }

        roleDao = model.Adapt<RoleDao>();
        await _thisRepository.InsertAsync(roleDao);

        return roleDao.id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task UpdateAsync(RoleDto model)
    {
        var roleDao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec && a.id != model.id);
        if (roleDao != null)
        {
            throw new BusinessException($"已存在名称为{model.namec}的角色！");
        }
        roleDao = await _thisRepository.GetByIdAsync(model.id);
        if (roleDao == null)
        {
            throw new BusinessException("无效的角色信息！");
        }

        roleDao = CommonUtils.Adapt(model, roleDao);
        roleDao.names = roleDao.namec;
        await _thisRepository.UpdateAsync(roleDao);
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

