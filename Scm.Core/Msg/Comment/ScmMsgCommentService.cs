using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Msg.Comment.Dvo;
using Com.Scm.Msg.CommentDetail.Dvo;
using Com.Scm.Msg.CommentHeader.Dvo;
using Com.Scm.Service;
using Com.Scm.Ur;
using Com.Scm.Utils;

using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Msg.Comment
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Msg")]
    public class ScmMsgCommentService : ApiService
    {
        private readonly SugarRepository<CommentHeaderDao> _headerRepository;
        private readonly SugarRepository<CommentDetailDao> _detailRepository;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerRepository"></param>
        /// <param name="detailRepository"></param>
        /// <param name="resHolder"></param>
        /// <returns></returns>
        public ScmMsgCommentService(SugarRepository<CommentHeaderDao> headerRepository,
            SugarRepository<CommentDetailDao> detailRepository,
            IResHolder resHolder)
        {
            _headerRepository = headerRepository;
            _detailRepository = detailRepository;
            _ResHolder = resHolder;
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<CommentHeaderDvo> GetAsync(ViewComment model)
        {
            var headerDvo = await _headerRepository
                .AsQueryable()
                .Where(a => a.codec == model.codec && a.row_status == ScmRowStatusEnum.Enabled)
                .Select<CommentHeaderDvo>()
                .FirstAsync();

            if (headerDvo != null)
            {
                var detailList = await _detailRepository
                    .AsQueryable()
                    .Where(a => a.comment_id == headerDvo.id && a.row_status == ScmRowStatusEnum.Enabled)
                    .OrderBy(a => a.id, SqlSugar.OrderByType.Desc)
                    .Select<CommentDetailDvo>()
                    .ToListAsync();

                Prepare(detailList);
                headerDvo.details = detailList;
            }

            return headerDvo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        private void Prepare(List<CommentDetailDvo> list)
        {
            foreach (var item in list)
            {
                item.update_names = _ResHolder.GetResNames<UserDao>(item.update_user);

                var createDao = _ResHolder.GetRes<UserDao>(item.create_user);
                if (createDao != null)
                {
                    item.create_names = createDao.names;
                    item.namec = createDao.namec;
                    item.avatar = createDao.avatar;
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task PostAsync(PostComment model)
        {
            var headerDao = await _headerRepository.GetFirstAsync(a => a.codec == model.codec && a.row_status == ScmRowStatusEnum.Enabled);
            if (headerDao == null)
            {
                throw new BusinessException($"无效的评论对象！");
            }

            if (IsValidId(model.rid))
            {
                var detailDao = await _detailRepository.GetByIdAsync(model.rid);
                if (detailDao == null)
                {
                    throw new BusinessException($"无效的回复对象！");
                }
            }

            var dao = model.Adapt<CommentDetailDao>();
            dao.comment_id = headerDao.id;

            await _detailRepository.InsertAsync(dao);

            headerDao.qty += 1;
            await _headerRepository.UpdateAsync(headerDao);
        }

        /// <summary>
        /// 打分
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task PostScoreAsync(SaveScoreRequest request)
        {
            if (!IsValidId(request.id))
            {
                throw new BusinessException($"无效的回复对象！");
            }

            var dao = await _headerRepository.GetByIdAsync(request.id);
            dao.score_value += request.score;
            dao.score_count += 1;

            await _headerRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task PostLikesAsync(SaveLikeRequest request)
        {
            if (!IsValidId(request.id))
            {
                throw new BusinessException($"无效的回复对象！");
            }

            var dao = await _detailRepository.GetByIdAsync(request.id);
            dao.likes += 1;

            await _detailRepository.UpdateAsync(dao);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ViewComment
    {
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sort { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PostComment
    {
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// 回复ID
        /// </summary>
        public long rid { get; set; }

        /// <summary>
        /// 引用ID
        /// </summary>
        public long pid { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PostScore
    {
        /// <summary>
        /// 
        /// </summary>
        public long score { get; set; }
    }
}
