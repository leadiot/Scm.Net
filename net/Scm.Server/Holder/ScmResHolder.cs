using Com.Scm.Dao;
using SqlSugar;

namespace Com.Scm.Holder
{
    public class ScmResHolder : IResHolder
    {
        private ISqlSugarClient _SqlClient;
        private Dictionary<long, IResDao> _LocalDict = new Dictionary<long, IResDao>();
        private static Dictionary<long, IResDao> _StaticDict = new Dictionary<long, IResDao>();

        public ScmResHolder(ISqlSugarClient sqlClient)
        {
            _SqlClient = sqlClient;
        }

        public void Clear()
        {
            _LocalDict.Clear();
        }

        public T GetRes<T>(long id, bool cached = false) where T : IResDao
        {
            return (T)GetResDao<T>(id, cached);
        }

        public string GetResNames<T>(long id, bool cached = false) where T : IResDao
        {
            var obj = GetRes<T>(id);
            return obj?.names;
        }

        public string GetResNamec<T>(long id, bool cached = false) where T : IResDao
        {
            var obj = GetRes<T>(id);
            return obj?.namec;
        }

        public void Remove(long id)
        {
            _LocalDict.Remove(id);
        }

        private IResDao GetResDao<T>(long id, bool cached = false) where T : IResDao
        {
            return cached ? GetStaticDao<T>(id) : GetLocalDao<T>(id);
        }

        private IResDao GetLocalDao<T>(long id) where T : IResDao
        {
            if (_LocalDict.ContainsKey(id))
            {
                return (T)_LocalDict[id];
            }

            var item = _SqlClient.Queryable<T>().Where(a => a.id == id).First();
            if (item != null)
            {
                _LocalDict[id] = item;
            }
            return item;
        }

        private IResDao GetStaticDao<T>(long id) where T : IResDao
        {
            if (_StaticDict.ContainsKey(id))
            {
                return (T)_LocalDict[id];
            }

            var item = _SqlClient.Queryable<T>().Where(a => a.id == id).First();
            if (item != null)
            {
                // 控制缓存不要太大
                if (_StaticDict.Count > 4096)
                {
                    _StaticDict.Clear();
                }
                _StaticDict[id] = item;
            }
            return item;
        }
    }
}
