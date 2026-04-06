using Com.Scm.Utils;
using FreeRedis;

namespace Com.Scm.Cache.Impl
{
    public class RedisService : ICacheService
    {
        private static RedisService _Instance;
        private ICacheConfig _Config;

        private RedisClient _Cache;

        public RedisService(ICacheConfig config)
        {
            _Config = config;
            //if (_Config.Type != "Redis")
            //{

            //}
            _Cache = new RedisClient(_Config.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        public static ICacheService GetInstance(ICacheConfig config)
        {
            if (_Instance == null)
            {
                _Instance = new RedisService(config);
            }
            return _Instance;
        }

        /// <summary>
        /// 查询Redis信息
        /// </summary>
        /// <param name="redisKey"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetCache<T>(string redisKey) where T : class, new()
        {
            var redisStr = _Cache.Get(redisKey);
            return !string.IsNullOrEmpty(redisStr) ? redisKey.AsJsonObject<T>() : null;
        }

        /// <summary>
        /// 查询Redis信息
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public void SetJson<T>(string redisKey, T t)
        {
            _Cache.Set(redisKey, t.ToJsonString());
        }

        public void SetCache<T>(string redisKey, T t)
        {
            _Cache.Set(redisKey, t);
        }

        public bool Exists(string key)
        {
            return _Cache.Exists(key);
        }

        public string GetCache(string key)
        {
            return _Cache.Get(key);
        }

        public void SetCache(string key, string value)
        {
            _Cache.Set(key, value);
        }

        public void SetCache(string key, object value)
        {
            _Cache.Set(key, value);
        }

        public void SetCache(string key, object value, int timeoutSeconds)
        {
            _Cache.Set(key, value, timeoutSeconds);
        }

        public void SetCache(string key, object value, DateTimeOffset expirationTime)
        {
            throw new NotImplementedException();
        }

        public void SetCache(string key, object value, TimeSpan t)
        {
            _Cache.Set(key, value, t);
        }

        public void RemoveCache(string key)
        {
            _Cache.Del(key);
        }

        public void Dispose()
        {
            _Cache.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}