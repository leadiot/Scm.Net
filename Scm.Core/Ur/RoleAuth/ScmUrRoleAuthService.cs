using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Ur.RoleAuth.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Ur.RoleAuth;

/// <summary>
/// 授权表服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "ur")]
public class ScmUrRoleAuthService : ApiService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    /// <param name="userRoleRepository"></param>
    /// <param name="roleConflictRepository"></param>
    /// <param name="roleRepository"></param>
    public ScmUrRoleAuthService(ISqlSugarClient sqlClient)
    {
        _SqlClient = sqlClient;
    }

    /// <summary>
    /// 根据角色获得权限
    /// </summary>
    /// <returns></returns>
    [HttpGet("{roleId}")]
    public async Task<List<RoleAuthDto>> GetByRoleAsync(long roleId)
    {
        return await _SqlClient.Queryable<RoleAuthDao>()
            .Where(m => m.role_id == roleId && m.types == Enums.ScmRoleAuthTypesEnum.RoleMenu)
            .OrderBy(m => m.id)
            .Select<RoleAuthDto>()
            .ToListAsync();
    }

    /// <summary>
    /// 根据用户获得授权的角色
    /// </summary>
    /// <returns></returns>
    [HttpGet("{userId}")]
    public async Task<List<string>> GetByUserAsync(long userId)
    {
        var model = await _SqlClient.Queryable<UserRoleDao>()
            .Where(m => m.user_id == userId)
            .ToListAsync();
        return model.Select(m => m.role_id.ToString()).ToList();
    }

    /// <summary>
    /// 为用户授权多个角色
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task AddRoleAsync(SysAuthorityAdminByRoleParam param)
    {
        //根据角色查询互斥内容
        var roleConflict = await _SqlClient.Queryable<RoleConflictDao>().ToListAsync();
        if (roleConflict.Count > 0)
        {
            if (roleConflict.Any(item => param.RoleArr.Contains(item.rolea_id.ToString()) && param.RoleArr.Contains(item.roleb_id.ToString())))
            {
                throw new BusinessException("角色存在互斥关系，无法授权！~");
            }
        }

        //查询角色列表
        var roleList = await _SqlClient.Queryable<RoleDao>().Where(m => param.RoleArr.Contains(m.id.ToString())).ToListAsync();
        //查询已授权角色信息
        var adminRoleList = await _SqlClient.Queryable<UserRoleDao>()
            .Where(m => param.RoleArr.Contains(m.role_id.ToString()))
            .ToListAsync();
        foreach (var item in roleList)
        {
            if (item is { max_length: 0 })
            {
                continue;
            }

            if (item != null && item.max_length <= adminRoleList.Count(m => m.role_id == item.id))
            {
                throw new BusinessException("[" + item.namec + "]-已达到角色设置最大边界值！~");
            }
        }

        var addRole = new List<UserRoleDao>();
        var adminRoleArr = await _SqlClient.Queryable<UserRoleDao>().Where(m => param.AdminArr.Contains(m.user_id.ToString())).ToListAsync();
        foreach (var item in param.AdminArr)
        {
            var roleIds = adminRoleArr.Where(m => m.user_id == long.Parse(item))
                .Select(m => m.role_id.ToString()).ToList();
            var differenceQuery = param.RoleArr.Except(roleIds);
            addRole.AddRange(differenceQuery.Select(row => new UserRoleDao() { user_id = long.Parse(item), role_id = long.Parse(row) }));
        }
        if (addRole.Count > 0)
        {
            await _SqlClient.InsertAsync(addRole);
        }
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(SysAuthorityParam model)
    {
        await _SqlClient.Deleteable<RoleAuthDao>()
            .Where(m => m.role_id == model.RoleId && m.types == Enums.ScmRoleAuthTypesEnum.RoleMenu)
            .ExecuteCommandAsync();
        var list = model.Menus.Select(item => new RoleAuthDao()
        {
            role_id = model.RoleId,
            auth_id = item.MenuId,
            api = item.Api,
            types = Enums.ScmRoleAuthTypesEnum.RoleMenu
        }).ToList();

        return await _SqlClient.InsertAsync<RoleAuthDao>(list) > 0;
    }
}