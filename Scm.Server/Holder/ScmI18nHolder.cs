using Com.Scm.Enums;
using Com.Scm.I18n;
using Com.Scm.Sys.I18n;
using SqlSugar;

namespace Com.Scm.Holder
{
    public class ScmI18nHolder : I18nHolder
    {
        private readonly ISqlSugarClient _SqlClient;
        private readonly Cache.ICacheService _cache;
        private const string CACHE_KEY = "i18n:all";

        public ScmI18nHolder(ISqlSugarClient sqlClient, Cache.ICacheService cache)
        {
            _SqlClient = sqlClient;
            _cache = cache;
        }

        public bool Translate<T>(List<T> daoList, string lang) where T : I18nItem
        {
            if (daoList == null || daoList.Count == 0 || lang == null) return false;

            var dic = Load(lang);
            foreach (var dao in daoList)
            {
                var key = dao.GetKey();
                if (key == null) continue;

                if (dic.ContainsKey(key))
                {
                    dao.SetLang(dic[key]);
                }
            }
            return true;
        }

        /// <summary>
        /// 加载某语言全部翻译（带缓存）
        /// </summary>
        public Dictionary<string, string> Load(string lang, bool useCache = false)
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

            var list = _SqlClient.Queryable<ScmSysI18nDao>()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled && a.lang == lang)
                .Select(a => new { a.key, a.value })
                .ToList();

            //var dict = list.ToDictionary(a => a.key, a => a.value);
            var dict = new Dictionary<string, string>();
            foreach (var item in list)
            {
                dict[item.key] = item.value;
            }
            _cache.SetCache(cacheKey, dict, TimeSpan.FromMinutes(30));
            return dict;
        }
    }
}
