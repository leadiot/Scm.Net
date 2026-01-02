using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Msg.Notice.Dvo;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Ur;
using Com.Scm.Ur.User.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Msg.Notice;

/// <summary>
/// 通知模块服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Msg")]
public class ScmMsgNoticeService : ApiService
{
    private readonly SugarRepository<NoticeDao> _thisRepository;
    private readonly SugarRepository<NoticeSummaryDao> _summaryRepository;
    private readonly SugarRepository<NoticeRecipientDao> _recipientRepository;
    private readonly SugarRepository<NoticeAttachmentDao> _attachmentRepository;
    private readonly SugarRepository<NoticeReaderDao> _readerRepository;
    private readonly SugarRepository<NoticeSenderDao> _senderRepository;
    private readonly ScmContextHolder _jwtHolder;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    /// <param name="summaryRepository"></param>
    /// <param name="recipientRepository"></param>
    /// <param name="attachmentRepository"></param>
    /// <param name="readerRepository"></param>
    /// <param name="senderRepository"></param>
    /// <param name="jwtHolder"></param>
    /// <param name="userService"></param>
    public ScmMsgNoticeService(SugarRepository<NoticeDao> thisRepository
        , SugarRepository<NoticeSummaryDao> summaryRepository
        , SugarRepository<NoticeRecipientDao> recipientRepository
        , SugarRepository<NoticeAttachmentDao> attachmentRepository
        , SugarRepository<NoticeReaderDao> readerRepository
        , SugarRepository<NoticeSenderDao> senderRepository
        , ScmContextHolder jwtHolder
        , IUserHolder userService)
    {
        _thisRepository = thisRepository;
        _summaryRepository = summaryRepository;
        _recipientRepository = recipientRepository;
        _attachmentRepository = attachmentRepository;
        _jwtHolder = jwtHolder;
        _readerRepository = readerRepository;
        _senderRepository = senderRepository;
        _UserHolder = userService;
    }

    /// <summary>
    /// 查询所有——分页== 默认查询收件箱
    /// status=（1=草稿2=存档3=删除）
    /// type=(1=已发送 2=收件箱  3=带文件)
    /// ReadStatus=(1=未读 2=已读)
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<NoticeDvo>> GetPagesAsync(SearchRequest param)
    {
        // 收件箱
        if (param.folder == SearchFolderEnum.Inbox)
        {
            return await SearchSjAsync(param);
        }
        // 发件箱
        if (param.folder == SearchFolderEnum.Outbox)
        {
            return await SearchFjAsync(param);
        }
        // 草稿箱
        if (param.status == SearchStatusEnum.Draft)
        {
            return await SearchCgAsync(param);
        }
        // 删除
        if (param.status == SearchStatusEnum.Droped)
        {
            return await SearchSjAsync(param);
        }
        // 归档
        if (param.status == SearchStatusEnum.Archived)
        {
            return await SearchSjAsync(param);
        }

        return await SearchSjAsync(param);
        //var query = await _thisRepository.AsQueryable()
        //    .WhereIF(!string.IsNullOrEmpty(param.key), m => m.title.Contains(param.key))
        //    //// 草稿等
        //    //.WhereIF(param.cat_status == NoticeCatEnum.Zc, m => m.cat == param.cat_status && m.send_user == param.id)
        //    //.WhereIF(param.cat_status == NoticeCatEnum.Cg, m => m.cat == param.cat_status && m.send_user == param.id)
        //    //.WhereIF(param.cat_status == NoticeCatEnum.Sc, m => m.cat == param.cat_status && m.send_user != param.id && !m.IsSend && (SqlFunc.ToString(m.recipients).Contains(param.id.ToString()) || SqlFunc.ToString(m.recipients) == "[0]"))
        //    //.WhereIF(param.cat_status == NoticeCatEnum.Gd, m => m.cat == param.cat_status && !m.IsSend && (SqlFunc.ToString(m.recipients).Contains(param.id.ToString()) || SqlFunc.ToString(m.recipients) == "[0]"))
        //    //// 查询发件人=已发送
        //    //.WhereIF(param.Type == 1, m => m.send_user == param.id && m.cat == 0 && m.IsSend)
        //    //// 查询收件人=收件箱
        //    //.WhereIF(param.Type == 2, m => (SqlFunc.ToString(m.recipients).Contains(param.id.ToString()) || SqlFunc.ToString(m.recipients) == "[0]") && m.send_user != param.id && m.cat == 0 && !m.IsSend)
        //    //// 查询 带文件的通知
        //    //.WhereIF(param.Type == 3, m => !SqlFunc.IsNullOrEmpty(m.files) && (SqlFunc.ToString(m.recipients).Contains(param.id.ToString()) || SqlFunc.ToString(m.recipients) == "[0]") && m.send_user != param.id && m.cat == 0 && !m.IsSend)
        //    // 未读=1 已读=2  全部=0
        //    .WhereIF(param.read_status == NoticeReadEnum.Unread, m => SqlFunc.Subqueryable<NoticeReaderDao>().Where(s => s.notice_id == m.id && s.user_id == param.id && s.read_qty == 0).Any())
        //    .WhereIF(param.read_status == NoticeReadEnum.Readed, m => SqlFunc.Subqueryable<NoticeReaderDao>().Where(s => s.notice_id == m.id && s.user_id == param.id && s.read_qty != 0).Any())
        //    .OrderByDescending(m => m.id)
        //    .ToPageAsync(param.Page, param.Limit);
        //var result = query.Adapt<PageResult<NoticeSearchResponse>>();

        ////查询已读
        //var noticeIds = result.Items.Select(m => m.id).ToList();
        //var readList = await _readerRepository.GetListAsync(m => m.read_qty != 0 && noticeIds.Contains(m.notice_id) && m.user_id == param.id);

        //foreach (var item in result.Items)
        //{
        //    //是否已读
        //    item.is_read = readList.Any(m => m.notice_id == item.id);
        //}
        //return result;
    }

    /// <summary>
    /// 收件箱
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<NoticeDvo>> SearchSjAsync(SearchRequest param)
    {
        var userId = _jwtHolder.GetToken().user_id;

        var result = await _readerRepository.AsQueryable()
            .Where(a => a.user_id == userId)
            .WhereIF(param.status == SearchStatusEnum.Droped, a => a.is_del == true)
            .WhereIF(param.status == SearchStatusEnum.Archived, a => a.is_arc == true)
            .WhereIF(param.readed == NoticeReadedEnum.Unread, a => a.unread == true)
            .WhereIF(param.readed == NoticeReadedEnum.Readed, a => a.unread == false)
            .WhereIF(!string.IsNullOrEmpty(param.key), m => SqlFunc.Subqueryable<NoticeDao>().Where(s => s.id == m.id && s.title.Contains(param.key)).Any())
            .OrderByDescending(a => a.id)
            .ToPageAsync(param.page, param.limit);

        var page = new ScmSearchPageResponse<NoticeDvo>();
        var list = new List<NoticeDvo>();
        for (var i = 0; i < result.Items.Count; i += 1)
        {
            var tmp = result.Items[i];
            var dao = await _thisRepository.GetByIdAsync(tmp.notice_id);
            if (dao == null)
            {
                continue;
            }

            var dto = result.Items[i];

            var dvo = new NoticeDvo();
            dvo.id = tmp.id;
            dvo.title = dao.title;
            dvo.content = dao.content;

            var userDao = _UserHolder.GetUser(dao.send_user);
            dvo.sender = userDao.Adapt<SimpleUserDvo>();
            list.Add(dvo);
        }

        page.Items = list;
        return page;
    }

    /// <summary>
    /// 发件箱
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<NoticeDvo>> SearchFjAsync(SearchRequest param)
    {
        var userId = _jwtHolder.GetToken().user_id;

        var result = await _senderRepository.AsQueryable()
            .Where(a => a.user_id == userId && a.handle == ScmHandleEnum.Done)
            .WhereIF(!string.IsNullOrEmpty(param.key), m => SqlFunc.Subqueryable<NoticeDao>().Where(s => s.id == m.id && s.title.Contains(param.key)).Any())
            .OrderByDescending(a => a.id)
            .ToPageAsync(param.page, param.limit);

        var page = new ScmSearchPageResponse<NoticeDvo>();
        var list = new List<NoticeDvo>();
        for (var i = 0; i < result.Items.Count; i += 1)
        {
            var tmp = result.Items[i];
            var dao = await _thisRepository.GetByIdAsync(tmp.notice_id);
            if (dao == null)
            {
                continue;
            }

            var dvo = new NoticeDvo();
            dvo.id = tmp.id;
            dvo.title = dao.title;
            dvo.content = dao.content;
            dvo.unread = false;

            var userDao = _UserHolder.GetUser(dao.send_user);
            dvo.sender = userDao.Adapt<SimpleUserDvo>();
            list.Add(dvo);
        }

        page.Items = list;
        return page;
    }

    /// <summary>
    /// 草稿箱
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<NoticeDvo>> SearchCgAsync(SearchRequest param)
    {
        var userId = _jwtHolder.GetToken().user_id;

        var result = await _senderRepository.AsQueryable()
            .Where(a => a.user_id == userId && a.handle == ScmHandleEnum.Todo)
            .WhereIF(!string.IsNullOrEmpty(param.key), m => SqlFunc.Subqueryable<NoticeDao>().Where(s => s.id == m.id && s.title.Contains(param.key)).Any())
            .OrderByDescending(a => a.id)
            .ToPageAsync(param.page, param.limit);

        var page = new ScmSearchPageResponse<NoticeDvo>();
        var list = new List<NoticeDvo>();
        for (var i = 0; i < result.Items.Count; i += 1)
        {
            var tmp = result.Items[i];
            var dao = await _thisRepository.GetByIdAsync(tmp.notice_id);
            if (dao == null)
            {
                continue;
            }

            var dvo = new NoticeDvo();
            dvo.id = tmp.id;
            dvo.title = dao.title;
            dvo.content = dao.content;
            dvo.unread = false;

            var userDao = _UserHolder.GetUser(dao.send_user);
            dvo.sender = userDao.Adapt<SimpleUserDvo>();
            list.Add(dvo);
        }

        page.Items = list;
        return page;
    }

    /// <summary>
    /// 查询未读、草稿、删除、存档统计数据
    /// </summary>
    /// <returns></returns>
    public async Task<NoticeSummaryDto> GetSummaryAsync()
    {
        var userId = _jwtHolder.GetToken().user_id;

        var summaryDao = await _summaryRepository.GetByIdAsync(userId);
        if (summaryDao == null)
        {
            summaryDao = new NoticeSummaryDao();
            summaryDao.id = userId;

            summaryDao.total = await _readerRepository.CountAsync(a => a.user_id == userId);
            summaryDao.unread = await _readerRepository.CountAsync(a => a.user_id == userId && a.unread == true);
            summaryDao.draft = await _senderRepository.CountAsync(a => a.user_id == userId && a.handle == ScmHandleEnum.Todo);
            summaryDao.archived = await _readerRepository.CountAsync(a => a.user_id == userId && a.is_arc == true);
            summaryDao.deleted = await _readerRepository.CountAsync(a => a.user_id == userId && a.is_del == true);

            await _summaryRepository.InsertAsync(summaryDao);
        }

        return summaryDao.Adapt<NoticeSummaryDto>();
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ReadViewResponse> GetViewAsync(long id)
    {
        var userId = _jwtHolder.GetToken().user_id;

        var readerDao = await _readerRepository.GetFirstAsync(a => a.id == id && a.user_id == userId);
        if (readerDao == null)
        {
            return null;
        }

        // 更新标识
        readerDao.read_qty += 1;
        readerDao.unread = false;
        await _readerRepository.AsUpdateable(readerDao).UpdateColumns(a => new { a.read_qty, a.unread, a.update_time, a.update_user }).ExecuteCommandAsync();

        // 查询对象
        var noticeDao = await _thisRepository.GetByIdAsync(readerDao.notice_id);
        if (noticeDao == null)
        {
            return null;
        }
        var dvo = noticeDao.Adapt<ReadViewResponse>();

        // 获取发件人
        dvo.sender = await GetUserInfo(noticeDao.send_user);

        // 获取收件人列表
        var recipients = await _recipientRepository.AsQueryable().Where(a => a.notice_id == noticeDao.id).Select<NoticeRecipientDvo>().ToListAsync();
        var query = _thisRepository.Change<UserDao>().AsQueryable();
        for (var i = 0; i < recipients.Count; i++)
        {
            var recipient = recipients[i];
            var user = await query.Where(a => a.id == recipient.user_id).Select<SimpleUserDvo>().FirstAsync();
            if (user != null)
            {
                recipient = CommonUtils.Adapt(user, recipient);
            }
        }
        dvo.recipients = recipients;

        await _summaryRepository.AsUpdateable()
            .SetColumns(a => new NoticeSummaryDao { unread = a.unread - 1, update_time = TimeUtils.GetUnixTime(), update_user = userId })
            .Where(a => a.id == userId)
            .ExecuteCommandAsync();

        return dvo;
    }

    private async Task<SimpleUserDvo> GetUserInfo(long id)
    {
        return await _thisRepository.Change<UserDao>().AsQueryable()
            .Where(a => a.id == id)
            .Select<SimpleUserDvo>()
            .FirstAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ReadEditResponse> GetEditAsync(long id)
    {
        var userId = _jwtHolder.GetToken().user_id;

        var senderDao = await _senderRepository.GetFirstAsync(a => a.id == id && a.user_id == userId);
        if (senderDao == null)
        {
            return null;
        }

        // 查询对象
        var noticeDao = await _thisRepository.GetByIdAsync(senderDao.notice_id);
        if (noticeDao == null)
        {
            return null;
        }
        var dvo = noticeDao.Adapt<ReadEditResponse>();

        // 获取收件人列表
        var recipients = await _recipientRepository.AsQueryable().Where(a => a.notice_id == noticeDao.id).Select<NoticeRecipientDvo>().ToListAsync();
        var query = _thisRepository.Change<UserDao>().AsQueryable();
        for (var i = 0; i < recipients.Count; i++)
        {
            var recipient = recipients[i];
            var user = await query.Where(a => a.id == recipient.user_id).FirstAsync();
            if (user != null)
            {
                recipient = CommonUtils.Adapt(user, recipient);
            }
        }
        dvo.recipients = recipients;

        return dvo;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task AddAsync(NoticeUpdateRequest request)
    {
        var sendId = _jwtHolder.GetToken().user_id;
        if (sendId == 0)
        {
            throw new BusinessException("发件人不能为空");
        }

        var isSend = request.operate == NoticeUpdateEnum.Send;

        // 邮件
        var dao = new NoticeDao();
        dao.send_user = sendId;
        if (isSend)
        {
            dao.send_time = TimeUtils.GetUnixTime();
        }
        dao.title = request.title;
        dao.content = request.content;
        //dao.cat = 0;
        await _thisRepository.InsertAsync(dao);

        // 发件人
        var senderDao = new NoticeSenderDao();
        senderDao.notice_id = dao.id;
        senderDao.user_id = sendId;
        senderDao.handle = ScmHandleEnum.Todo;
        await _senderRepository.InsertAsync(senderDao);

        // 收件人
        var recipients = new List<NoticeRecipientDao>();
        foreach (var recipient in request.recipients)
        {
            recipients.Add(new NoticeRecipientDao { notice_id = dao.id, user_id = recipient });
        }
        await _recipientRepository.InsertRangeAsync(recipients);

        // 附件
        var attachments = new List<NoticeAttachmentDao>();
        foreach (var file in request.files)
        {
            attachments.Add(new NoticeAttachmentDao { notice_id = dao.id, namec = file.Name, url = file.Url });
        }
        await _attachmentRepository.InsertRangeAsync(attachments);

        // 发送处理
        if (isSend)
        {
            await DoSend(request, senderDao);
        }

        await UpdateSummary(sendId);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task UpdateAsync(NoticeUpdateRequest dto)
    {
        var sendId = _jwtHolder.GetToken().user_id;
        if (sendId == 0)
        {
            throw new BusinessException("发件人不能为空");
        }

        // 邮件
        var dao = await _thisRepository.GetByIdAsync(dto.id);
        if (dao == null)
        {
            return;
        }
        dao.send_user = sendId;
        //dao.send_time = DateTime.Now;
        dao.title = dto.title;
        dao.content = dto.content;
        await _thisRepository.UpdateAsync(dao);

        // 发件人
        var senderDao = new NoticeSenderDao();
        senderDao.notice_id = dao.id;
        senderDao.handle = ScmHandleEnum.Todo;
        await _senderRepository.InsertAsync(senderDao);

        // 收件人
        await _recipientRepository.DeleteAsync(a => a.notice_id == dto.id);
        var recipients = new List<NoticeRecipientDao>();
        foreach (var recipient in dto.recipients)
        {
            recipients.Add(new NoticeRecipientDao { notice_id = dao.id, user_id = recipient });
        }
        await _recipientRepository.InsertRangeAsync(recipients);

        // 附件
        await _attachmentRepository.DeleteAsync(a => a.notice_id == dto.id);
        var attachments = new List<NoticeAttachmentDao>();
        foreach (var file in dto.files)
        {
            attachments.Add(new NoticeAttachmentDao { notice_id = dao.id, namec = file.Name, url = file.Url });
        }
        await _attachmentRepository.InsertRangeAsync(attachments);

        // 发送处理
        if (dto.operate == NoticeUpdateEnum.Send)
        {
            await DoSend(dto, senderDao);
        }

        await UpdateSummary(sendId);
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="dao"></param>
    /// <returns></returns>
    private async Task DoSend(NoticeUpdateRequest dto, NoticeSenderDao dao)
    {
        dao.handle = ScmHandleEnum.Doing;
        await _senderRepository.AsUpdateable(dao).UpdateColumns(a => new { a.handle, a.update_time, a.update_user }).ExecuteCommandAsync();

        var readed = new List<NoticeReaderDao>();
        foreach (var recipient in dto.recipients)
        {
            readed.Add(new NoticeReaderDao { notice_id = dao.notice_id, user_id = recipient });
        }
        await _readerRepository.InsertRangeAsync(readed);

        // 追加收件人未读信息
        await _summaryRepository.AsUpdateable()
            .SetColumns(a => new NoticeSummaryDao { unread = a.unread + 1 })
            .Where(a => dto.recipients.Contains(a.id))
            .ExecuteCommandAsync();

        dao.handle = ScmHandleEnum.Done;
        await _senderRepository.AsUpdateable(dao).UpdateColumns(a => new { a.handle, a.update_time, a.update_user }).ExecuteCommandAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    private async Task<NoticeSummaryDao> UpdateSummary(long userId)
    {
        var summaryDao = await _summaryRepository.GetByIdAsync(userId);
        if (summaryDao == null)
        {
            summaryDao = new NoticeSummaryDao();
            summaryDao.id = userId;

            await _summaryRepository.InsertAsync(summaryDao);
        }

        summaryDao.total = await _readerRepository.CountAsync(a => a.user_id == userId);
        summaryDao.unread = await _readerRepository.CountAsync(a => a.user_id == userId && a.unread == true);
        summaryDao.draft = await _senderRepository.CountAsync(a => a.user_id == userId && a.handle == ScmHandleEnum.Todo);
        summaryDao.archived = await _readerRepository.CountAsync(a => a.user_id == userId && a.is_arc == true);
        summaryDao.deleted = await _readerRepository.CountAsync(a => a.user_id == userId && a.is_del == true);

        await _summaryRepository.UpdateAsync(summaryDao);
        return summaryDao;
    }

    /// <summary>
    /// 更改状态（1=草稿2=存档3=删除）
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> ChangeArchiveAsync([FromBody] List<long> ids)
    {
        var userId = _jwtHolder.GetToken().user_id;

        var qty = await _readerRepository.AsUpdateable()
                 .SetColumns(a => new NoticeReaderDao { read_qty = a.read_qty + 1, is_arc = true, update_time = TimeUtils.GetUnixTime(), update_user = userId })
                 .Where(a => ids.Contains(a.id))
                 .ExecuteCommandAsync();

        await UpdateSummary(userId);

        return qty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> CancelArchiveAsync([FromBody] List<long> ids)
    {
        var userId = _jwtHolder.GetToken().user_id;

        var qty = await _readerRepository.AsUpdateable()
                 .SetColumns(a => new NoticeReaderDao { read_qty = a.read_qty + 1, is_arc = false, update_time = TimeUtils.GetUnixTime(), update_user = userId })
                 .Where(a => ids.Contains(a.id))
                 .ExecuteCommandAsync();

        await UpdateSummary(userId);

        return qty;
    }

    /// <summary>
    /// 设置已读
    /// 空数组为设置全部已读
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task<int> ChangeReadedAsync([FromBody] List<long> ids)
    {
        var userId = _jwtHolder.GetToken().user_id;

        var qty = await _readerRepository.AsUpdateable()
                 .SetColumns(a => new NoticeReaderDao { read_qty = a.read_qty + 1, unread = false, update_time = TimeUtils.GetUnixTime(), update_user = userId })
                 .Where(a => ids.Contains(a.id))
                 .ExecuteCommandAsync();

        await UpdateSummary(userId);

        return qty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<int> ChangeUnreadAsync([FromBody] List<long> ids)
    {
        var userId = _jwtHolder.GetToken().user_id;

        var qty = await _readerRepository.AsUpdateable()
                 .SetColumns(a => new NoticeReaderDao { read_qty = a.read_qty + 1, unread = true, update_time = TimeUtils.GetUnixTime(), update_user = userId })
                 .Where(a => ids.Contains(a.id))
                 .ExecuteCommandAsync();

        await UpdateSummary(userId);

        return qty;
    }

    /// <summary>
    /// 取消已读
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    public async Task ChangeAsync([FromBody] List<long> ids)
    {
        var userId = _jwtHolder.GetToken().user_id;

        await _readerRepository.DeleteAsync(m => ids.Contains(m.notice_id) && m.user_id == userId);
    }


    /// <summary>
    /// 删除,支持批量
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<int> DeleteAsync([FromBody] List<long> ids)
    {
        return await _readerRepository
            .AsUpdateable()
            .SetColumns(a => new NoticeReaderDao { is_arc = true })
            .Where(a => ids.Contains(a.id))
            .ExecuteCommandAsync();
    }
}
