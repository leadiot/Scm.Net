using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Sys.Dic;
using Com.Scm.Sys.DicDetail.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.DicDetail;

/// <summary>
/// 字典信息表服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysDicDetailService : ApiService
{
    private readonly SugarRepository<DicDetailDao> _thisRepository;
    private SugarRepository<DicHeaderDao> _headerRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    /// <param name="headerRepository"></param>
    public ScmSysDicDetailService(SugarRepository<DicDetailDao> thisRepository, SugarRepository<DicHeaderDao> headerRepository)
    {
        _thisRepository = thisRepository;
        _headerRepository = headerRepository;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<DicDetailDto>> GetPagesAsync(SearchRequest request)
    {
        if (!string.IsNullOrEmpty(request.codec))
        {
            var dicHeaderDao = await _headerRepository.GetSingleAsync(m => m.codec == request.codec);
            if (dicHeaderDao != null)
            {
                request.id = dicHeaderDao.id;
            }
        }

        var result = await _thisRepository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
            .WhereIF(IsNormalId(request.id), m => m.dic_header_id == request.id)
            .WhereIF(request.tag != 0, m => m.tag == request.tag)
            .Select<DicDetailDto>()
            .ToPageAsync(request.page, request.limit);
        return result;
    }

    /// <summary>
    /// 查询所有
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<DicDetailDto>> GetListAsync(SearchRequest request)
    {
        if (!string.IsNullOrEmpty(request.codec))
        {
            var typeModel = await _headerRepository.AsQueryable()
                .WhereIF(request.tag != 0, m => m.types == request.tag)
                .FirstAsync(m => m.codec == request.codec);
            if (typeModel != null)
            {
                request.id = typeModel.id;
            }
        }
        var result = await _thisRepository.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
            .WhereIF(IsNormalId(request.id), m => m.dic_header_id == request.id)
            .WhereIF(request.tag != 0, m => m.tag == request.tag)
            .Select<DicDetailDto>()
            .ToListAsync();
        return result;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<DicDetailDto> GetAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<DicDetailDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task AddAsync(DicDetailDto model)
    {
        var isAny = await _thisRepository.IsAnyAsync(m => m.dic_header_id == model.dic_header_id && m.namec == model.namec);
        if (isAny)
        {
            throw new BusinessException("名称不能重复~");
        }
        await _thisRepository.InsertReturnSnowflakeIdAsync(model.Adapt<DicDetailDao>());
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(DicDetailDto model)
    {
        var list = await _thisRepository.GetListAsync(m => m.dic_header_id == model.dic_header_id);
        if (list != null && list.Count > 0)
        {
            var tmpDao = list.Find(a => a.value == model.value && a.id != model.id);
            if (tmpDao != null)
            {
                throw new BusinessException($"已存在值为{model.value}的子项！");
            }
            tmpDao = list.Find(a => a.codec == model.codec && a.id != model.id);
            if (tmpDao != null)
            {
                throw new BusinessException($"已存在代码为{model.codec}的子项！");
            }
            tmpDao = list.Find(a => a.namec == model.namec && a.id != model.id);
            if (tmpDao != null)
            {
                throw new BusinessException($"已存在名称为{model.namec}的子项！");
            }
        }

        var dao = await _thisRepository.GetByIdAsync(model.id);
        if (dao == null)
        {
            throw new BusinessException("无效的子项信息！");
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
