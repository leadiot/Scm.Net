using Com.Scm.Cache;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Com.Scm.Hubs
{
    [EnableCors(ScmEnv.SCM_CORS)]
    public class ScmHub : Hub
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ICacheService _cacheService;

        public ScmHub(IHttpContextAccessor accessor, ICacheService cacheService)
        {
            _accessor = accessor;
            _cacheService = cacheService;
        }

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            LogUtils.Info("SignalR已连接-ConnectionId：" + connectionId);

            if (_accessor.HttpContext != null)
            {
                var token = _accessor.HttpContext.Request.Query["access_token"];
                LogUtils.Info("SignalR已连接-Token：" + token);

                var jwtToken = JwtUtils.SerializeJwt(token);
                LogUtils.Info("SignalR已连接-User：" + jwtToken.user_name);

                var user = new ClientUser()
                {
                    Id = jwtToken.user_id,
                    Name = jwtToken.user_name,
                    ConnectionId = connectionId,
                    Time = DateTime.Now
                };

                var userList = _cacheService.GetCache<List<ClientUser>>(KeyUtils.ONLINEUSERS);
                if (userList == null)
                {
                    userList = new List<ClientUser>();
                    userList.Add(user);
                    _cacheService.SetCache(KeyUtils.ONLINEUSERS, userList);
                }
                else
                {
                    var now = userList.FirstOrDefault(m => m.Id == jwtToken.user_id);
                    if (now != null)
                    {
                        Context.Items.Remove(now.ConnectionId);
                        userList.Remove(now);
                    }
                    userList.Add(user);
                    _cacheService.SetCache(KeyUtils.ONLINEUSERS, userList);
                }
            }

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            var userList = _cacheService.GetCache<List<ClientUser>>(KeyUtils.ONLINEUSERS);
            if (userList != null)
            {
                var now = userList.FirstOrDefault(m => m.ConnectionId == connectionId);
                if (now != null)
                {
                    userList.Remove(now);
                }
                _cacheService.SetCache(KeyUtils.ONLINEUSERS, userList);
            }

            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="user"></param>
        [HubMethodName("SendKickOut")]
        public async Task SendKickOut(string user)
        {
            var list = _cacheService.GetCache<List<ClientUser>>(KeyUtils.ONLINEUSERS);
            if (list != null)
            {
                var now = list.FirstOrDefault(m => m.Id == long.Parse(user));
                if (now != null)
                {
                    Context.Items.Remove(now.ConnectionId);
                    list.Remove(now);
                }
                _cacheService.SetCache(KeyUtils.ONLINEUSERS, list);
            }
            await Clients.All.SendAsync("ReceiveKickout", "out", user);
        }

        /// <summary>
        /// 发送消息变更提醒
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public async Task SendMessage(long userId, int qty)
        {
            var response = new ScmResultResponse<int>() { Data = qty };
            await SendAsync(userId, "ReceiveMessage", response);
        }

        /// <summary>
        /// 发送通知变更提醒
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public async Task SendNotice(long userId, int qty)
        {
            var response = new ScmResultResponse<int>() { Data = qty };
            await SendAsync(userId, "ReceiveNotice", response);
        }

        private async Task SendAsync<T>(long userId, string method, ScmResultResponse<T> response)
        {
            var list = _cacheService.GetCache<List<ClientUser>>(KeyUtils.ONLINEUSERS);
            if (list != null)
            {
                var now = list.FirstOrDefault(a => a.Id == userId);
                if (now != null)
                {
                    await Clients.Client(now.ConnectionId).SendAsync(method, response);
                    return;
                }
            }
        }

        private async Task SendAsync<T>(string method, ScmResultResponse<T> response)
        {
            await Clients.All.SendAsync(method, response);
        }
    }
}