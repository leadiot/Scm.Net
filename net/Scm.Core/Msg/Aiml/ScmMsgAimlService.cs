using Com.Scm.Aiml;
using Com.Scm.Config;
using Com.Scm.Hubs;
using Com.Scm.Msg.Aiml.Dvo;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Com.Scm.Msg.Aiml
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Msg")]
    public class ScmMsgAimlService : ApiService
    {
        private readonly ScmContextHolder _contextHolder;
        private readonly IHubContext<ScmHub> _hubContext;
        private readonly EnvConfig _envConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextHolder"></param>
        /// <param name="hubContext"></param>
        /// <param name="envConfig"></param>
        /// <returns></returns>
        public ScmMsgAimlService(ScmContextHolder contextHolder,
            IHubContext<ScmHub> hubContext,
            EnvConfig envConfig)
        {
            _contextHolder = contextHolder;
            _hubContext = hubContext;
            _envConfig = envConfig;
        }


        /// <summary>
        /// 聊天
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public AimlChatResponse ChatAsync(AimlChatRequest request)
        {
            var token = _contextHolder.GetToken();

            var bot = GetRobot(token);
            var man = GetHuman(token, bot);

            var aimlRequest = new AimlRequest(request.content, man, bot);
            var aimlResponse = bot.Chat(aimlRequest, false);

            SetRobot(token, bot);
            SetHuman(token, man);

            var response = new AimlChatResponse();
            response.content = aimlResponse.ToString();

            return response;
        }

        private void SetRobot(ScmToken token, Robot robot)
        {
            AimlObjects.SetRobot(robot);
        }

        private Robot GetRobot(ScmToken token)
        {
            var bot = AimlObjects.GetRobot();
            if (bot == null)
            {
                var path = _envConfig.GetDataPath("aiml");
                if (!Directory.Exists(path))
                {
                    var template = _envConfig.GetDataPath("aiml");
                    FileUtils.CopyDir(template, path);
                }
                bot = new Robot(path);
                bot.LoadConfig();
                bot.LoadAIML();
            }

            return bot;
        }

        private void SetHuman(ScmToken token, Human human)
        {
            AimlObjects.SetHuman(token.user_id, human);
        }

        private Human GetHuman(ScmToken token, Robot bot)
        {
            var man = AimlObjects.GetHuman(token.user_id);
            if (man == null)
            {
                man = new Human(token.user_name, bot);
            }

            return man;
        }
    }
}
