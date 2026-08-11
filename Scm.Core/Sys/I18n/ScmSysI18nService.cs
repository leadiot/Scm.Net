using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.I18n;

/// <summary>
/// 全局多语言翻译服务
/// </summary>
[ApiExplorerSettings(GroupName = "sys")]
public class ScmSysI18nService : IApiService
{
    private readonly SugarRepository<ScmSysI18nDao> _repo;
    private readonly Cache.ICacheService _cache;
    private const string CACHE_KEY = "i18n:all";

    public ScmSysI18nService(SugarRepository<ScmSysI18nDao> repo, Cache.ICacheService cache)
    {
        _repo = repo;
        _cache = cache;
    }

    /// <summary>
    /// 拉取某语言的全部翻译（前端用）
    /// </summary>
    [HttpGet]
    public async Task<Dictionary<string, string>> GetAsync(string lang)
    {
        return await LoadTranslations(lang);
    }

    /// <summary>
    /// 业务层批量翻译：传入 key 列表 + lang，返回 key->value
    /// </summary>
    public async Task<Dictionary<string, string>> TranslateAsync(List<string> keys, string lang)
    {
        if (keys == null || keys.Count == 0)
        {
            return new Dictionary<string, string>();
        }

        var all = await LoadTranslations(lang);
        var result = new Dictionary<string, string>();
        foreach (var key in keys.Distinct())
        {
            all.TryGetValue(key, out var val);
            result[key] = val;
        }
        return result;
    }

    /// <summary>
    /// 加载某语言全部翻译（带缓存）
    /// </summary>
    private async Task<Dictionary<string, string>> LoadTranslations(string lang)
    {
        if (string.IsNullOrWhiteSpace(lang))
        {
            return new Dictionary<string, string>();
        }

        var cacheKey = $"{CACHE_KEY}:{lang}";
        var cached = _cache.GetCache<Dictionary<string, string>>(cacheKey);
        if (cached != null)
        {
            return cached;
        }

        var list = await _repo.AsQueryable()
            .Where(a => a.row_status == ScmRowStatusEnum.Enabled && a.lang == lang)
            .Select(a => new { a.key, a.value })
            .ToListAsync();

        var dict = list.ToDictionary(a => a.key, a => a.value);
        _cache.SetCache(cacheKey, dict, TimeSpan.FromMinutes(30));
        return dict;
    }

    /// <summary>
    /// 写入后清缓存
    /// </summary>
    public async Task<bool> AddAsync(ScmSysI18nDao dao)
    {
        var ok = await _repo.InsertAsync(dao);
        if (ok) _cache.RemoveCache($"{CACHE_KEY}:{dao.lang}");
        return ok;
    }

    public async Task UpdateAsync(ScmSysI18nDao dao)
    {
        await _repo.UpdateAsync(dao);
        _cache.RemoveCache($"{CACHE_KEY}:{dao.lang}");
    }

    public async Task<bool> DeleteAsync(string ids)
    {
        var idList = ids.ToListLong();
        var langs = await _repo.AsQueryable()
            .Where(a => idList.Contains(a.id))
            .Select(a => a.lang)
            .ToListAsync();
        var ok = await _repo.DeleteAsync(a => idList.Contains(a.id));
        foreach (var l in langs.Distinct())
        {
            _cache.RemoveCache($"{CACHE_KEY}:{l}");
        }
        return true;
    }
}