using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Ur.RoleConflict.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Ur.SysRoleConflict;

/// <summary>
/// 角色互斥表服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Ur")]
public class ScmUrRoleConflictService : ApiService
{
    private readonly SugarRepository<RoleConflictDao> _thisRepository;
    private readonly SugarRepository<RoleDao> _roleRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmUrRoleConflictService(SugarRepository<RoleConflictDao> thisRepository,
        IResHolder resHolder,
        SugarRepository<RoleDao> roleRepository)
    {
        _thisRepository = thisRepository;
        _ResHolder = resHolder;
        _roleRepository = roleRepository;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<RoleConflictDvo>> GetPagesAsync(ScmSearchPageRequest param)
    {
        var query = await _thisRepository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(param.key), m => m.remark.Contains(param.key))
            .OrderByDescending(m => m.id)
            .Select<RoleConflictDvo>()
            .ToPageAsync(param.page, param.limit);

        Prepare(query.Items);
        return query;
    }

    private void Prepare(List<RoleConflictDvo> list)
    {
        foreach (var item in list)
        {
            Prepare(item);

            var roleDao = _roleRepository.GetById(item.rolea_id);
            item.rolea_names = roleDao?.names;

            roleDao = _roleRepository.GetById(item.roleb_id);
            item.roleb_names = roleDao?.names;
        }
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<RoleConflictDto> GetAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<RoleConflictDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(RoleConflictDto model)
    {
        var dao = await _thisRepository.GetFirstAsync(a => a.rolea_id == model.rolea_id && a.roleb_id == model.roleb_id);
        if (dao != null)
        {
            throw new BusinessException("已存在相同的互斥规则！");
        }

        //var roleA = await _thisRepository.GetByIdAsync(model.rolea_id);
        //if (roleA == null)
        //{
        //    throw new BusinessException("无效的角色A！");
        //}

        //var roleB = await _thisRepository.GetByIdAsync(model.roleb_id);
        //if (roleB == null)
        //{
        //    throw new BusinessException("无效的角色B！");
        //}

        dao = model.Adapt<RoleConflictDao>();
        return await _thisRepository.InsertAsync(dao);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(RoleConflictDvo model)
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
    /// 删除,支持批量
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync([FromBody] List<long> ids) =>
        await _thisRepository.DeleteAsync(m => ids.Contains(m.id));
}
