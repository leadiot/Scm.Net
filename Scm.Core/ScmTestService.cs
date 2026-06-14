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
        private IScmTokenHolder _ScmHolder;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlClient"></param>
        /// <param name="nasConfig"></param>
        /// <param name="resHolder"></param>
        public ScmTestService(ISqlSugarClient sqlClient,
            EnvConfig envConfig,
            IScmTokenHolder scmHolder,
            IResHolder resHolder)
        {
            _SqlClient = sqlClient;
            _EnvConfig = envConfig;
            _ScmHolder = scmHolder;
            _ResHolder = resHolder;
            //_MessageService = messageService;
        }

        public string PostTestAsync()
        {
            var token = _ScmHolder.GetToken();
            LogUtils.Info("PostSync", "TOKEN", token.ToJsonString());

            return token.ToJsonString();
        }
    }
}
