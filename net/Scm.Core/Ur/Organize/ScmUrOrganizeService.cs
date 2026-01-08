using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Ur.Organize.Dvo;
using Com.Scm.Ur.Organize.Rnr;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Ur.Organize;

/// <summary>
/// 服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Ur")]
public class ScmUrOrganizeService : ApiService
{
    private readonly SugarRepository<OrganizeDao> _thisRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmUrOrganizeService(SugarRepository<OrganizeDao> thisRepository, IUserHolder userService)
    {
        _thisRepository = thisRepository;
        _ResHolder = userService;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<OrganizeDvo>> GetPagesAsync(ScmSearchPageRequest param)
    {
        var query = await _thisRepository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(param.key), m => m.namec.Contains(param.key))
            .Select<OrganizeDvo>()
            .ToPageAsync(param.page, param.limit);

        Prepare(query.Items);
        return query;
    }

    /// <summary>
    /// 查询所有
    /// </summary>
    /// <returns></returns>
    public async Task<List<OrganizeDvo>> GetListAsync(ScmSearchPageRequest param)
    {
        var list = await _thisRepository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(param.key), m => m.namec.Contains(param.key))
            .OrderBy(m => m.id, OrderByType.Asc)
            .Select<OrganizeDvo>()
            .ToListAsync();

        Prepare(list);
        return list;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<OrganizeDto> GetAsync(long id)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.id == id)
            .Select<OrganizeDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<OrganizeDto> GetEditAsync(long id)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.id == id)
            .Select<OrganizeDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<OrganizeDvo> GetViewAsync(long id)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.id == id)
            .Select<OrganizeDvo>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(OrganizeDto model)
    {
        var organizeDao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
        if (organizeDao != null)
        {
            throw new BusinessException($"已存在编码为{model.codec}的组织！");
        }
        organizeDao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
        if (organizeDao != null)
        {
            throw new BusinessException($"已存在名称为{model.codec}的组织！");
        }

        organizeDao = model.Adapt<OrganizeDao>();
        await _thisRepository.InsertAsync(organizeDao);

        return organizeDao.id;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(OrganizeDto model)
    {
        var organizeDao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
        if (organizeDao != null)
        {
            throw new BusinessException($"已存在编码为{model.codec}的组织！");
        }
        organizeDao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec && a.id != model.id);
        if (organizeDao != null)
        {
            throw new BusinessException($"已存在名称为{model.codec}的组织！");
        }

        organizeDao = await _thisRepository.GetByIdAsync(model.id);
        if (organizeDao == null)
        {
            throw new BusinessException("无效的组织信息！");
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
        var idList = ids.ToListLong();
        return await _thisRepository
            .AsUpdateable()
            .SetColumns(a => a.row_delete == Enums.ScmDeleteEnum.Yes)
            .Where(a => idList.Contains(a.id))
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新主管
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> ChangeOwnerAsync(ChangeOwnerRequest request)
    {
        var dao = await _thisRepository.GetByIdAsync(request.id);
        if (dao == null)
        {
            throw new BusinessException("无效的组织信息");
        }

        var userDao = await _thisRepository.Change<UserDao>().GetByIdAsync(request.owner_id);
        if (userDao == null)
        {
            throw new BusinessException("无效的人员信息");
        }

        dao.owner_id = userDao.id;
        dao.owner_names = userDao.names;

        return await _thisRepository.UpdateAsync(dao);
    }
}

