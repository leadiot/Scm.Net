using Microsoft.Extensions.Caching.Memory;

namespace Com.Scm.Cache.Impl
{
    public class MemoryService : ICacheService
    {
        private static ICacheService _Instance;
        private readonly IMemoryCache _Cache;
        private ICacheConfig _Config;

        public MemoryService(ICacheConfig config)
        {
            _Config = config;
            //if (_Config.Type != "Memory")
            //{

            //}
            _Cache = new MemoryCache(new MemoryCacheOptions());
        }

        public MemoryService(IMemoryCache cache, ICacheConfig config)
        {
            _Cache = cache;
            _Config = config;
        }

        public static ICacheService GetInstance(ICacheConfig config)
        {
            if (_Instance == null)
            {
                _Instance = new MemoryService(new MemoryCache(new MemoryCacheOptions()), config);
            }
            return _Instance;
        }

        /// <summary>
        /// 是否存在此缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _Cache.TryGetValue<object>(key, out _);
        }

        /// <summary>
        /// 取得缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCache<T>(string key) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _Cache.TryGetValue(key, out T v);
            return v;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCache(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (_Cache.TryGetValue(key, out _))
            {
                _Cache.Remove(key);
            }

            _Cache.Set(key, value);
        }

        /// <summary>
        /// 设置缓存,绝对过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationMinute">间隔分钟</param>
        /// MemoryCacheService.Default.SetCache("test", "RedisCache works!", 30);
        public void SetCache(string key, object value, int timeoutSeconds)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (_Cache.TryGetValue(key, out _))
                _Cache.Remove(key);
            DateTime now = DateTime.Now;
            TimeSpan ts = now.AddSeconds(timeoutSeconds) - DateTime.Now;
            _Cache.Set(key, value, ts);
        }

        /// <summary>
        /// 设置缓存,绝对过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime">DateTimeOffset 结束时间</param>
        /// MemoryCacheService.Default.SetCache("test", "RedisCache works!", DateTimeOffset.Now.AddSeconds(30));
        public void SetCache(string key, object value, DateTimeOffset expirationTime)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (_Cache.TryGetValue(key, out object v))
                _Cache.Remove(key);

            _Cache.Set(key, value, expirationTime);
        }

        /// <summary>
        /// 设置缓存,相对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="t"></param>
        /// MemoryCacheService.Default.SetCache("test", "MemoryCache works!",TimeSpan.FromSeconds(30));
        public void SetCache(string key, object value, TimeSpan t)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (_Cache.TryGetValue(key, out object v))
                _Cache.Remove(key);

            _Cache.Set(key, value, new MemoryCacheEntryOptions()
            {
                SlidingExpiration = t
            });
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCache(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            _Cache.Remove(key);
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            if (_Cache != null)
            {
                _Cache.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public string GetCache(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _Cache.TryGetValue(key, out string v);
            return v;
        }

        public void SetCache(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (_Cache.TryGetValue(key, out _))
            {
                _Cache.Remove(key);
            }

            _Cache.Set(key, value);
        }
    }
}