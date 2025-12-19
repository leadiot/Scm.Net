using Com.Scm.Adm.Terminal;
using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Terminal.Dvo;
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

            terminalDao.access_token = TextUtils.RandomString(16);
            terminalDao.refresh_token = TextUtils.RandomString(16);
            terminalDao.expires = TimeUtils.GetUnixTime(DateTime.UtcNow.AddMonths(1));
            terminalDao.os = request.os;
            terminalDao.mac = request.mac;
            terminalDao.binded = ScmBoolEnum.True;
            await _SqlClient.UpdateAsync(terminalDao);

            token.terminal_id = terminalDao.id;
            token.access_token = terminalDao.access_token;
            token.refresh_token = terminalDao.refresh_token;
            token.expires = terminalDao.expires;

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

            terminalDao.access_token = TextUtils.RandomString(16);
            terminalDao.refresh_token = TextUtils.RandomString(16);
            terminalDao.expires = TimeUtils.GetUnixTime(DateTime.UtcNow.AddMonths(1));
            await _SqlClient.UpdateAsync(terminalDao);

            //token.terminal_id = terminalDao.id;
            token.access_token = terminalDao.access_token;
            token.refresh_token = terminalDao.refresh_token;
            token.expires = terminalDao.expires;

            return token;
        }
    }
}
