using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Hubs;
using Com.Scm.Jwt;
using Com.Scm.Msg.Message.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SqlSugar;

namespace Com.Scm.Msg.Message;

/// <summary>
/// 留言消息表服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Msg")]
public class ScmMsgMessageService : ApiService
{
    /// <summary>
    /// APPID
    /// </summary>
    public const long APP_ID = 1714913846931623936L;

    private readonly SugarRepository<MessageDao> _thisRepository;
    private readonly SugarRepository<MessageTagDao> _tagRepository;
    private readonly JwtContextHolder _jwtHolder;
    private readonly ITagService _tagService;
    private readonly IHubContext<ScmHub> _hubContext;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    /// <param name="tagRepository"></param>
    /// <param name="operatorService"></param>
    /// <param name="tagService"></param>
    /// <param name="cacheService"></param>
    /// <param name="hubContext"></param>
    public ScmMsgMessageService(SugarRepository<MessageDao> thisRepository,
        SugarRepository<MessageTagDao> tagRepository,
        JwtContextHolder jwtHolder,
        ITagService tagService,
        Cache.ICacheService cacheService,
        IHubContext<ScmHub> hubContext)
    {
        _thisRepository = thisRepository;
        _tagRepository = tagRepository;
        _jwtHolder = jwtHolder;
        _tagService = tagService;
        _CacheService = cacheService;
        _hubContext = hubContext;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<MessageDvo>> GetPagesAsync(SearchRequest param)
    {
        var token = _jwtHolder.GetToken();

        var query = _thisRepository.AsQueryable()
            .Where(m => m.user_id == token.user_id)
            .WhereIF(!string.IsNullOrEmpty(param.key), m => m.title.Contains(param.key) || m.remark.Contains(param.key));

        if (!string.IsNullOrWhiteSpace(param.cat))
        {
            switch (param.cat)
            {
                // 已读消息
                case "msg11":
                    query = query.Where(a => a.isread && !a.is_del);
                    break;
                // 未读消息
                case "msg12":
                    query = query.Where(a => !a.isread && !a.is_del);
                    break;
                // 回收站
                case "msg13":
                    query = query.Where(a => a.is_del);
                    break;
                default:
                    if (ScmUtils.IsValidId(param.cat))
                    {
                        var tagId = long.Parse(param.cat);
                        query = query.Where(a => SqlFunc.Subqueryable<MessageTagDao>().Where(b => b.message_id == a.id && b.tag_id == tagId).Any());
                    }
                    break;
            }
        }

        var result = await query.OrderByDescending(m => m.id)
            .Select<MessageDvo>()
            .ToPageAsync(param.page, param.limit);

        return result;
    }

    /// <summary>
    /// 获取未读列表
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<List<MessageDvo>> GetUnreadAsync(SearchRequest param)
    {
        var token = _jwtHolder.GetToken();

        return await _thisRepository.AsQueryable()
            .Where(m => m.user_id == token.user_id)
            .WhereIF(!string.IsNullOrEmpty(param.key), m => m.title.Contains(param.key) || m.remark.Contains(param.key))
            .Where(a => !a.isread && !a.is_del)
            .OrderByDescending(m => m.id)
            .Select<MessageDvo>()
            .ToListAsync();
    }

    /// <summary>
    /// 查询消息的汇总
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<MessageSummaryDvo> GetTotalAsync()
    {
        var user = _jwtHolder.GetToken();

        var allCount = await _thisRepository.CountAsync(m => m.user_id == user.user_id);
        var unReadCount = await _thisRepository.CountAsync(m => !m.isread && m.user_id == user.user_id);
        var recycleCount = await _thisRepository.CountAsync(m => m.row_delete == Enums.ScmDeleteEnum.Yes && m.user_id == user.user_id);
        return new MessageSummaryDvo()
        {
            AllCount = allCount,
            UnReadCount = unReadCount,
            RecycleCount = recycleCount
        };
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<MessageDvo> GetEditAsync(long id)
    {
        var dvo = await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<MessageDvo>()
            .FirstAsync();

        if (dvo != null)
        {
            dvo.tags = await _tagRepository.AsQueryable()
                .Where(a => a.message_id == dvo.id)
                .Select(a => new TagDvo { id = a.tag_id, label = a.label })
                .ToListAsync();
        }

        return dvo;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<MessageDvo> GetViewAsync(long id)
    {
        var dao = await _thisRepository.GetByIdAsync(id);
        return dao.Adapt<MessageDvo>();
    }


    /// <summary>
    /// 阅读消息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<MessageDvo> GetReadAsync(long id)
    {
        var user = _jwtHolder.GetToken();

        var dao = await _thisRepository.GetByIdAsync(id);
        if (dao.user_id == user.user_id)
        {
            await _thisRepository.UpdateAsync(m => new MessageDao() { isread = true }, m => m.id == id);
        }
        return dao.Adapt<MessageDvo>();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(MessageDto request)
    {
        var dao = request.Adapt<MessageDao>();
        dao.types = Enums.ScmMessageTypeEnum.User;

        var result = await _thisRepository.InsertAsync(dao);
        if (result)
        {
            if (request.tags != null && request.tags.Count > 0)
            {
                var list = new List<MessageTagDao>();
                foreach (var tag in request.tags)
                {
                    var msgTagDao = new MessageTagDao();
                    msgTagDao.message_id = dao.id;
                    msgTagDao.tag_id = tag;
                    list.Add(msgTagDao);
                }
                await _tagRepository.InsertRangeAsync(list);
            }

            SendWss(request.user_id);
        }

        return result;
    }

    /// <summary>
    /// 通知客户端有数据更新
    /// </summary>
    /// <param name="userId"></param>
    private void SendWss(long userId)
    {
        var userList = _CacheService.GetCache<List<ClientUser>>(KeyUtils.ONLINEUSERS);
        if (userList == null)
        {
            return;
        }

        var user = userList.FirstOrDefault(a => a.Id == userId);
        if (user == null)
        {
            return;
        }

        var client = _hubContext.Clients.Client(user.ConnectionId);
        if (client == null)
        {
            return;
        }

        var response = new ScmResultResponse<bool>();
        response.SetSuccess(true);
        client.SendAsync("ReceiveMessage", response);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(MessageDto request)
    {
        var dao = await _thisRepository.GetByIdAsync(request.id);
        if (dao == null)
        {
            return false;
        }

        dao = request.Adapt(dao);
        dao.types = Enums.ScmMessageTypeEnum.User;

        var result = await _thisRepository.UpdateAsync(dao);
        if (result)
        {
            await _tagRepository.DeleteAsync(a => a.message_id == request.id);
            if (request.tags != null && request.tags.Count > 0)
            {
                var list = new List<MessageTagDao>();
                foreach (var tag in request.tags)
                {
                    var msgTagDao = new MessageTagDao();
                    msgTagDao.message_id = dao.id;
                    msgTagDao.tag_id = tag;
                    list.Add(msgTagDao);
                }
                await _tagRepository.InsertRangeAsync(list);
            }
        }
        return result;
    }

    /// <summary>
    /// 设置已读
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> ReadAsync(List<long> ids)
    {
        if (ids == null || ids.Count < 1)
        {
            return false;
        }

        return await _thisRepository.UpdateAsync(m => new MessageDao() { isread = true }, m => ids.Contains(m.id));
    }

    /// <summary>
    /// 设置已读
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> ReadAllAsync()
    {
        var user = _jwtHolder.GetToken();

        return await _thisRepository.UpdateAsync(m => new MessageDao() { isread = true }, m => m.user_id == user.user_id);
    }

    /// <summary>
    /// 删除到回收站
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> RecycleAsync(List<long> ids)
    {
        return await _thisRepository.UpdateAsync(m => new MessageDao() { is_del = true }, m => ids.Contains(m.id));
    }

    /// <summary>
    /// 删除,支持多个
    /// </summary>
    /// <param name="ids">逗号分隔</param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(string ids)
    {
        return await _thisRepository.DeleteAsync(m => ids.ToListLong().Contains(m.id));
    }
}
