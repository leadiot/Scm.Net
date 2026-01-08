using Com.Scm.Dsa;
using Com.Scm.Service;
using Com.Scm.Sys.Feedback.Dvo;
using Com.Scm.Sys.FeedbackDetail.Dvo;
using Com.Scm.Sys.FeedbackHeader.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Feedback
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Msg")]
    public class ScmSysFeedbackService : ApiService
    {
        private readonly SugarRepository<FeedbackHeaderDao> _headerRepository;
        private readonly SugarRepository<FeedbackDetailDao> _detailRepository;
        
        /// <summary>
        /// 
        /// </summary>
        public ScmSysFeedbackService(SugarRepository<FeedbackHeaderDao> headerRepository,
            SugarRepository<FeedbackDetailDao> detailRepository,
            IResHolder userService)
        {
            _headerRepository = headerRepository;
            _detailRepository = detailRepository;
            _ResHolder = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<FeedbackHeaderDvo>> GetPagesAsync(SearchPageRequest request)
        {
            var result = await _headerRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<FeedbackHeaderDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SaveAsync(SaveRequest request)
        {
            var detailDao = new FeedbackDetailDao();
            detailDao.header_id = request.header_id;
            detailDao.content = request.content;
            detailDao.customer_reply = true;
            await _detailRepository.InsertAsync(detailDao);

            var headerDao = await _headerRepository.GetByIdAsync(request.header_id);
            if (headerDao != null)
            {
                headerDao.customer_reply = true;
                await _headerRepository.UpdateAsync(headerDao);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<FeedbackHeaderDvo> ViewAsync(long id)
        {
            var headerDvo = await _headerRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<FeedbackHeaderDvo>()
                .FirstAsync();

            if (headerDvo != null)
            {
                headerDvo.details = await _detailRepository
                    .AsQueryable()
                    .Where(a => a.header_id == id)
                    .Select<FeedbackDetailDvo>()
                    .ToListAsync();
            }

            return headerDvo;
        }
    }
}
