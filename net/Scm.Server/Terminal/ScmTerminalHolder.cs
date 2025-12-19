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
        private static readonly ConcurrentDictionary<string, ScmTerminalToken> _TerminalList = new ConcurrentDictionary<string, ScmTerminalToken>();

        public ScmTerminalHolder(ISqlSugarClient sqlClient)
        {
            _SqlClient = sqlClient;
        }

        public ScmTerminalToken GetTerminalByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            ScmTerminalToken tokenInfo;
            if (_TerminalList.ContainsKey(token))
            {
                tokenInfo = _TerminalList[token];
                return tokenInfo;
            }

            tokenInfo = new ScmTerminalToken();
            var terminalDao = _SqlClient.Queryable<AdmTerminalDao>()
                .Where(a => a.access_token == token && a.row_status == Enums.ScmRowStatusEnum.Enabled)
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
            tokenInfo.expires = terminalDao.expires;
            tokenInfo.user_id = terminalDao.user_id;

            var userDao = _SqlClient.Queryable<UserDao>()
                .Where(a => a.id == terminalDao.id)
                .First();
            if (userDao == null)
            {
                tokenInfo.user_codes = userDao.codes;
                tokenInfo.user_names = userDao.names;
            }

            _TerminalList.TryAdd(token, tokenInfo);

            return tokenInfo;
        }

        public void Remote(string token)
        {
            if (token != null)
            {
                _TerminalList.Remove(token, out _);
            }
        }

        public void Clear()
        {
            _TerminalList.Clear();
        }
    }
}
