using Com.Scm.Config;
using Com.Scm.Filters;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm
{
    [NoAuditLog]
    [ApiExplorerSettings(GroupName = "Scm")]
    public class ScmTestService : AppService
    {
        private IJwtTokenHolder _JwtHolder;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlClient"></param>
        /// <param name="nasConfig"></param>
        /// <param name="resHolder"></param>
        public ScmTestService(ISqlSugarClient sqlClient,
            EnvConfig envConfig,
            IJwtTokenHolder jwtHolder,
            IResHolder resHolder)
        {
            _SqlClient = sqlClient;
            _EnvConfig = envConfig;
            _JwtHolder = jwtHolder;
            _ResHolder = resHolder;
            //_MessageService = messageService;
        }

        public string PostTestAsync()
        {
            var token = _JwtHolder.GetToken();
            LogUtils.Info("PostSync", "TOKEN", token.ToJsonString());

            return token.ToJsonString();
        }
    }
}
