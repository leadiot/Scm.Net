using Com.Scm.Aiml;
using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Hubs;
using Com.Scm.Msg.Aiml;
using Com.Scm.Msg.Chat.Message.Dvo;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SqlSugar;

namespace Com.Scm.Msg.Chat.Message
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Msg")]
    public class ScmMsgChatMessageService : ApiService
    {
        private readonly SugarRepository<ChatMsgHeaderDao> _headerRepository;
        private readonly SugarRepository<ChatMsgDetailDao> _detailRepository;
        private readonly SugarRepository<ChatGroupUserDao> _groupUserRepository;
        private readonly ScmContextHolder _contextHolder;
        private readonly IHubContext<ScmHub> _hubContext;
        private readonly AimlConfig _aimlConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerRepository"></param>
        /// <param name="detailRepository"></param>
        /// <param name="groupUserRepository"></param>
        /// <param name="resHolder"></param>
        /// <param name="contextHolder"></param>
        /// <param name="cacheService"></param>
        /// <param name="hubContext"></param>
        /// <param name="envConfig"></param>
        /// <param name="aimlConfig"></param>
        /// <returns></returns>
        public ScmMsgChatMessageService(SugarRepository<ChatMsgHeaderDao> headerRepository,
            SugarRepository<ChatMsgDetailDao> detailRepository,
            SugarRepository<ChatGroupUserDao> groupUserRepository,
            IResHolder resHolder,
            ScmContextHolder contextHolder,
            Cache.ICacheService cacheService,
            IHubContext<ScmHub> hubContext,
            EnvConfig envConfig,
            AimlConfig aimlConfig)
        {
            _headerRepository = headerRepository;
            _detailRepository = detailRepository;
            _groupUserRepository = groupUserRepository;
            _ResHolder = resHolder;
            _contextHolder = contextHolder;
            _CacheService = cacheService;
            _hubContext = hubContext;
            _EnvConfig = envConfig;
            _aimlConfig = aimlConfig;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ChatDetailDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _detailRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<ChatDetailDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ChatHeaderDvo>> GetListHeaderAsync(SearchRequest request)
        {
            var result = await _headerRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrWhiteSpace(request.key), a => a.namec.Contains(request.key))
                .OrderBy(a => a.id, SqlSugar.OrderByType.Desc)
                .Select<ChatHeaderDvo>()
                .ToListAsync();

            //Prepare(result);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ChatDetailDvo>> GetListDetailAsync(SearchRequest request)
        {
            var result = await _detailRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled && a.create_time >= request.last_time)
                .WhereIF(IsValidId(request.id), a => a.header_id == request.id)
                .OrderBy(a => a.id)
                .Select<ChatDetailDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ChatMsgDetailDto> GetAsync(long id)
        {
            var model = await _detailRepository.GetByIdAsync(id);
            return model.Adapt<ChatMsgDetailDto>();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ChatMsgDetailDto> GetEditAsync(long id)
        {
            return await _detailRepository
                .AsQueryable()
                .Select<ChatMsgDetailDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ChatDetailDvo> GetViewAsync(long id)
        {
            return await _detailRepository
                .AsQueryable()
                .Select<ChatDetailDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 创建会话
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ChatMsgHeaderDto> CreateAsync(CreateRequest request)
        {
            var token = _contextHolder.GetToken();

            var users = request.users ?? new List<long>();
            users.Insert(0, token.user_id);

            var groupClient = _detailRepository.Change<ChatGroupDao>();

            ChatGroupDao groupDao;
            ChatMsgHeaderDao headerDao;
            if (request.types == ScmChatGroupTypesEnum.Single)
            {
                var key = users[1].ToString();
                groupDao = await groupClient.GetFirstAsync(a => a.hash == key);
                if (groupDao == null)
                {
                    groupDao = await CreateGroup(request, groupClient);
                }
                headerDao = await _headerRepository.GetFirstAsync(a => a.group_id == groupDao.id);
                if (headerDao == null)
                {
                    headerDao = new ChatMsgHeaderDao();
                    headerDao.group_id = groupDao.id;
                    headerDao.namec = request.namec;
                    await _headerRepository.InsertAsync(headerDao);
                }
            }
            else
            {
                groupDao = await CreateGroup(request, groupClient);

                headerDao = new ChatMsgHeaderDao();
                headerDao.group_id = groupDao.id;
                headerDao.namec = request.namec;
                await _headerRepository.InsertAsync(headerDao);
            }

            return headerDao.Adapt<ChatMsgHeaderDto>();
        }

        private async Task<ChatGroupDao> CreateGroup(CreateRequest request, SimpleClient<ChatGroupDao> groupClient)
        {
            var groupUserClient = _detailRepository.Change<ChatGroupUserDao>();

            var groupDao = new ChatGroupDao();
            groupDao.types = request.types;
            groupDao.namec = request.namec;
            await groupClient.InsertAsync(groupDao);

            var userListDao = new List<ChatGroupUserDao>();
            foreach (var userId in request.users)
            {
                var groupUser = new ChatGroupUserDao();
                groupUser.group_id = groupDao.id;
                groupUser.user_id = userId;
                userListDao.Add(groupUser);
            }
            await groupUserClient.InsertRangeAsync(userListDao);

            request.users.Sort();
            var key = string.Join(",", request.users);
            if (request.types == ScmChatGroupTypesEnum.Groups)
            {
                key = Com.Scm.Utils.SecUtils.Md5(key);
            }

            groupDao.qty = userListDao.Count;
            groupDao.hash = key;
            await groupClient.UpdateAsync(groupDao);

            return groupDao;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task UpdateAsync(UpdateRequest request)
        {
            var dao = await _headerRepository.GetByIdAsync(request.id);
            if (dao == null)
            {
                throw new BusinessException($"无效的数据信息，更新失败！");
            }
            dao.namec = request.namec;
            await _headerRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus(_detailRepository, param.ids, param.status);
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> DeleteAsync(string ids)
        {
            return await DeleteRecord(_detailRepository, ids.ToListLong());
        }

        /// <summary>
        /// 聊天
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        [HttpPost]
        public async Task<ChatDetailDvo> ChatAsync(ChatRequest request)
        {
            var token = _contextHolder.GetToken();

            var headerDao = await _headerRepository.GetByIdAsync(request.id);
            if (headerDao == null)
            {
                return null;
            }

            var detailDao = new ChatMsgDetailDao();
            detailDao.user_id = token.user_id;
            detailDao.types = request.types;
            detailDao.content = request.content;
            detailDao.header_id = headerDao.id;
            await _detailRepository.InsertAsync(detailDao);

            await _headerRepository.UpdateAsync(headerDao);

            var message = new ChatMessage();
            message.id = detailDao.id.ToString();
            message.user_id = token.user_id.ToString();
            message.chat_id = request.id.ToString();
            message.types = request.types;
            message.content = request.content;
            message.create_time = detailDao.create_time.ToString();

            var groupId = headerDao.group_id;
            var userListDao = await _groupUserRepository.GetListAsync(a => a.group_id == groupId);
            SendWss(message, userListDao);

            var isRobot = request.HasRobot();
            var robotDao = userListDao.Find(a => a.user_id == UserDto.ROBOT_ID);
            if (isRobot || userListDao.Count < 3 && robotDao != null)
            {
                await ChatRobot(headerDao, userListDao, request, token);
            }

            return detailDao.Clone<ChatDetailDvo>();
        }

        private async Task ChatRobot(ChatMsgHeaderDao headerDao, List<ChatGroupUserDao> userListDao, ChatRequest request, ScmToken token)
        {
            var bot = GetRobot(token);
            var man = GetHuman(token, bot);

            var aimlRequest = new AimlRequest(request.content, man, bot);
            var aimlResponse = bot.Chat(aimlRequest, false);

            SetRobot(token, bot);
            SetHuman(token, man);

            var userId = UserDto.ROBOT_ID;

            var detailDao = new ChatMsgDetailDao();
            detailDao.user_id = userId;
            detailDao.types = request.types;
            detailDao.content = aimlResponse.ToString();
            detailDao.header_id = headerDao.id;
            await _detailRepository.InsertAsync(detailDao);

            await _headerRepository.UpdateAsync(headerDao);

            var message = new ChatMessage();
            message.id = detailDao.id.ToString();
            message.user_id = userId.ToString();
            message.chat_id = request.id.ToString();
            message.types = request.types;
            message.content = aimlResponse.ToString();
            message.create_time = detailDao.create_time.ToString();

            new Thread(() =>
            {
                Thread.Sleep(1000);
                SendWss(message, userListDao);
            }).Start();
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
                var path = _EnvConfig.GetDataPath(_aimlConfig.Folder);
                if (!Directory.Exists(path))
                {
                    var template = _EnvConfig.GetDataPath(_aimlConfig.Folder);
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
                var name = token.user_name ?? "小木";
                man = new Human(name, bot);
            }

            return man;
        }

        /// <summary>
        /// 通知客户端有数据更新
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userListDao"></param>
        private void SendWss(ChatMessage message, List<ChatGroupUserDao> userListDao)
        {
            var userList = _CacheService.GetCache<List<ClientUser>>(KeyUtils.ONLINEUSERS);
            if (userList == null)
            {
                return;
            }

            foreach (var userDao in userListDao)
            {
                var user = userList.FirstOrDefault(a => a.Id == userDao.user_id);
                if (user == null)
                {
                    continue;
                }

                var client = _hubContext.Clients.Client(user.ConnectionId);
                if (client == null)
                {
                    continue;
                }

                var response = new ScmResultResponse<ChatMessage>();
                response.SetSuccess(message);
                client.SendAsync("ChatMessage", response);
            }
        }
    }
}