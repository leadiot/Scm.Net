using Com.Scm.Utils;

namespace Com.Scm.Cache.Impl
{
    public class DictionaryService : ICacheService
    {
        private static ICacheService _Instance;

        private readonly Dictionary<string, string> _Cache;
        private ICacheConfig _Config;

        public DictionaryService(ICacheConfig config)
        {
            _Config = config;
            //if (_Config.Type != "Dictionary")
            //{

            //}
            _Cache = new Dictionary<string, string>();
        }

        public static ICacheService GetInstance(ICacheConfig config)
        {
            if (_Instance == null)
            {
                _Instance = new DictionaryService(config);
            }
            return _Instance;
        }

        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _Cache.ContainsKey(key);
        }

        public string GetCache(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!_Cache.ContainsKey(key))
            {
                return null;
            }

            return _Cache[key];
        }

        public void SetCache(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _Cache[key] = value;
        }

        public T GetCache<T>(string key) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (!_Cache.ContainsKey(key))
            {
                return null;
            }

            var redisStr = _Cache[key];
            return redisStr.AsJsonObject<T>();
        }

        public void SetCache(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _Cache[key] = value.ToJsonString();
        }

        public void SetCache(string key, object value, int timeoutSeconds)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _Cache[key] = value.ToJsonString();
        }

        public void SetCache(string key, object value, DateTimeOffset expirationTime)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _Cache[key] = value.ToJsonString();
        }

        public void SetCache(string key, object value, TimeSpan t)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _Cache[key] = value.ToJsonString();
        }

        public void RemoveCache(string key)
        {
            if (_Cache.ContainsKey(key))
            {
                _Cache.Remove(key);
            }
        }

        public void Dispose()
        {
            _Cache.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
