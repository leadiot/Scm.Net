using Com.Scm.Ur;
using Com.Scm.Utils;
using SqlSugar;

namespace Com.Scm.User
{
    public class ScmUserHolder : IResHolder
    {
        private readonly ISqlSugarClient _sqlClient;
        private static Dictionary<long, ScmUserInfo> _UserDict = new Dictionary<long, ScmUserInfo>();

        public ScmUserHolder(ISqlSugarClient sqlClient)
        {
            _sqlClient = sqlClient;
        }

        public ScmUserInfo GetUser(long id)
        {
            if (_UserDict.ContainsKey(id))
            {
                return _UserDict[id];
            }

            var dao = _sqlClient.GetById<UserDao>(id);
            if (dao == null)
            {
                return null;
            }

            var userInfo = dao.Adapt<ScmUserInfo>();
            _UserDict[id] = userInfo;
            return userInfo;
        }

        public string GetUserNames(long id)
        {
            return GetUser(id)?.names;
        }

        public void Remove(long id)
        {
            if (_UserDict.ContainsKey(id))
            {
                _UserDict.Remove(id);
            }
        }

        public void Clear()
        {
            _UserDict.Clear();
        }
    }
}
