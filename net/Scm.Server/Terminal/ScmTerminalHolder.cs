using Com.Scm.Adm.Terminal;
using Com.Scm.Ur;
using SqlSugar;
using System.Collections.Concurrent;

namespace Com.Scm.Terminal
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmTerminalHolder : ITerminalHolder
    {
        private readonly ISqlSugarClient _SqlClient;
        private static readonly ConcurrentDictionary<long, ScmTerminalToken> _TerminalList = new ConcurrentDictionary<long, ScmTerminalToken>();

        public ScmTerminalHolder(ISqlSugarClient sqlClient)
        {
            _SqlClient = sqlClient;
        }

        public ScmTerminalToken GetTerminal(long id)
        {
            ScmTerminalToken tokenInfo;
            if (_TerminalList.ContainsKey(id))
            {
                tokenInfo = _TerminalList[id];
                return tokenInfo;
            }

            tokenInfo = new ScmTerminalToken();
            var terminalDao = _SqlClient.Queryable<AdmTerminalDao>()
                .Where(a => a.id == id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .First();
            if (terminalDao == null)
            {
                return null;
            }

            tokenInfo.id = terminalDao.id;
            tokenInfo.names = terminalDao.names;
            tokenInfo.codes = terminalDao.codes;
            tokenInfo.access_token = terminalDao.access_token;
            tokenInfo.refresh_token = terminalDao.refresh_token;
            tokenInfo.expired = terminalDao.expired;
            tokenInfo.user_id = terminalDao.user_id;

            var userDao = _SqlClient.Queryable<UserDao>()
                .Where(a => a.id == terminalDao.id)
                .First();
            if (userDao == null)
            {
                tokenInfo.user_codes = userDao.codes;
                tokenInfo.user_names = userDao.names;
            }

            _TerminalList.TryAdd(id, tokenInfo);

            return tokenInfo;
        }

        public void Remote(long id)
        {
            _TerminalList.Remove(id, out _);
        }

        public void Clear()
        {
            _TerminalList.Clear();
        }
    }
}
