using Com.Scm.Cache;
using Com.Scm.Hubs;
using Com.Scm.Nas.Dto.Msg;
using Com.Scm.Utils;
using Microsoft.AspNetCore.SignalR;

namespace Com.Scm.Nas.Msg
{
    public class NasMessageService
    {
        private readonly IHubContext<ScmHub> _hubContext;
        private readonly ICacheService _cacheService;

        public NasMessageService(IHubContext<ScmHub> hubContext, ICacheService cacheService)
        {
            _hubContext = hubContext;
            _cacheService = cacheService;
        }

        /// <summary>
        /// 发送 NAS 消息给指定用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public async Task SendMessage(long userId, NasMessage message)
        {
            var response = new ScmResultResponse<NasMessage>() { Data = message };
            await SendAsync(userId, "ReceiveNasMessage", response);
        }

        /// <summary>
        /// 发送 NAS 消息给所有在线用户
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns></returns>
        public async Task SendMessageToAll(NasMessage message)
        {
            var response = new ScmResultResponse<NasMessage>() { Data = message };
            await _hubContext.Clients.All.SendAsync("ReceiveNasMessage", response);
        }

        /// <summary>
        /// 发送文件同步状态消息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="status">同步状态</param>
        /// <param name="message">状态消息</param>
        /// <returns></returns>
        public async Task SendSyncStatus(long userId, string filePath, NasSyncStatus status, string message = "")
        {
            var syncMessage = new NasSyncMessage
            {
                FilePath = filePath,
                Status = status,
                Message = message,
                Timestamp = DateTime.Now
            };

            var response = new ScmResultResponse<NasSyncMessage>() { Data = syncMessage };
            await SendAsync(userId, "ReceiveNasSyncStatus", response);
        }

        /// <summary>
        /// 发送文件夹变更消息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="changeType">变更类型</param>
        /// <returns></returns>
        public async Task SendFolderChange(long userId, string folderPath, NasFolderChangeType changeType)
        {
            var folderMessage = new NasFolderMessage
            {
                FolderPath = folderPath,
                ChangeType = changeType,
                Timestamp = DateTime.Now
            };

            var response = new ScmResultResponse<NasFolderMessage>() { Data = folderMessage };
            await SendAsync(userId, "ReceiveNasFolderChange", response);
        }

        private async Task SendAsync<T>(long userId, string method, ScmResultResponse<T> response)
        {
            var list = _cacheService.GetCache<List<ClientUser>>(KeyUtils.ONLINEUSERS);
            if (list != null)
            {
                var user = list.FirstOrDefault(a => a.Id == userId);
                if (user != null)
                {
                    await _hubContext.Clients.Client(user.ConnectionId).SendAsync(method, response);
                    return;
                }
            }
        }
    }
}
