using Com.Scm.Cache;
using Com.Scm.Config;

namespace Com.Scm.Server.Cache
{
    public class CacheConfig : ICacheConfig
    {
        public const string NAME = "Cache";

        public string Type { get; set; }
        public string Text { get; set; }

        public void Prepare(EnvConfig envConfig)
        {
            if (string.IsNullOrEmpty(Type))
            {
                Type = "dictionary";
            }
        }
    }
}
