using Com.Scm.Adm.Terminal;
using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Terminal.Dvo;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Terminal
{
    /// <summary>
    /// 终端服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "Scm")]
    public class TerminalService : ApiService
    {
        public TerminalService(ISqlSugarClient sqlClient
            , EnvConfig envConfig)
        {
            _SqlClient = sqlClient;
            _EnvConfig = envConfig;
        }

        /// <summary>
        /// 设备绑定
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<TokenResult> BindAsync(BindRequest request)
        {
            var token = new TokenResult();

            var terminalDao = await _SqlClient.Queryable<AdmTerminalDao>()
                .Where(a => a.codes == request.codes && a.row_status == ScmRowStatusEnum.Enabled)
                .FirstAsync();
            if (terminalDao == null)
            {
                throw new BusinessException("无效的终端或口令！");
            }

            if (terminalDao.pass != request.pass)
            {
                throw new BusinessException("无效的终端或口令！");
            }

            if (terminalDao.binded == ScmBoolEnum.True)
            {
                throw new BusinessException("设备已经绑定到其它终端！");
            }
            if (terminalDao.types != request.types)
            {
                throw new BusinessException("无效的终端类型！");
            }

            GenToken(terminalDao, token);
            terminalDao.mac = request.mac;
            terminalDao.os = request.os;
            terminalDao.dn = request.dn;
            terminalDao.dm = request.dm;
            terminalDao.binded = ScmBoolEnum.True;
            await _SqlClient.UpdateAsync(terminalDao);

            var userDao = await _SqlClient.Queryable<UserDao>()
                .Where(a => a.id == terminalDao.user_id && a.row_status == ScmRowStatusEnum.Enabled)
                .FirstAsync();
            if (userDao != null)
            {
                token.user_id = userDao.id;
                token.user_codes = userDao.codes;
                token.user_names = userDao.names;
                token.avatar = userDao.avatar;
            }

            return token;
        }

        /// <summary>
        /// 刷新授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [AllowAnonymous]
        public async Task<TokenResult> RefreshAsync(RefreshRequest request)
        {
            var token = new TokenResult();

            var terminalDao = await _SqlClient.Queryable<AdmTerminalDao>()
                .Where(a => a.id == request.terminal_id && a.row_status == ScmRowStatusEnum.Enabled)
                .FirstAsync();
            if (terminalDao == null)
            {
                throw new BusinessException("无效的终端信息！");
            }

            if (terminalDao.access_token != request.access_token ||
                terminalDao.refresh_token != request.refresh_token ||
                terminalDao.binded != ScmBoolEnum.True)
            {
                throw new BusinessException("无效的授权信息！");
            }

            if (terminalDao.IsExpired())
            {
                throw new BusinessException("无效的授权信息！");
            }

            GenToken(terminalDao, token);
            await _SqlClient.UpdateAsync(terminalDao);

            return token;
        }

        private void GenToken(AdmTerminalDao terminalDao, TokenResult token)
        {
            // 30天
            var expires = 60 * 60 * 24 * 30;

            terminalDao.access_token = TextUtils.RandomString(16, false);
            terminalDao.refresh_token = TextUtils.RandomString(16, false);
            terminalDao.expired = TimeUtils.GetUnixTime(DateTime.UtcNow.AddSeconds(expires));

            token.terminal_id = terminalDao.id;
            token.terminal_codes = terminalDao.codes;
            token.terminal_names = terminalDao.names;
            token.access_token = terminalDao.access_token;
            token.refresh_token = terminalDao.refresh_token;
            token.expires = expires;
        }
    }
}
