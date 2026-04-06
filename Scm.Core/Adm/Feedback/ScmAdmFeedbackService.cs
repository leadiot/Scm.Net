using Com.Scm.Adm.Feedback.Dao;
using Com.Scm.Adm.Feedback.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Service;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Adm.Feedback
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmAdmFeedbackService : ApiService
    {
        private readonly SugarRepository<AdmFeedbackHeaderDao> _headerRepository;
        private readonly SugarRepository<AdmFeedbackDetailDao> _detailRepository;

        /// <summary>
        /// 
        /// </summary>
        public ScmAdmFeedbackService(SugarRepository<AdmFeedbackHeaderDao> headerRespository,
            SugarRepository<AdmFeedbackDetailDao> detailRespository,
            IResHolder resHolder)
        {
            _headerRepository = headerRespository;
            _detailRepository = detailRespository;
            _ResHolder = resHolder;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<AdmFeedbackHeaderDvo>> GetPagesAsync(SearchRequest request)
        {
            var result = await _headerRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<AdmFeedbackHeaderDvo>()
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
            var detailDao = new AdmFeedbackDetailDao();
            detailDao.header_id = request.header_id;
            detailDao.content = request.content;
            detailDao.system_reply = true;
            await _detailRepository.InsertAsync(detailDao);

            var headerDao = await _headerRepository.GetByIdAsync(request.header_id);
            if (headerDao != null)
            {
                headerDao.system_reply = true;
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
        public async Task<AdmFeedbackHeaderDvo> ViewAsync(long id)
        {
            var headerDvo = await _headerRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<AdmFeedbackHeaderDvo>()
                .FirstAsync();

            if (headerDvo != null)
            {
                headerDvo.user_names = _ResHolder.GetResNames<UserDao>(headerDvo.user_id);

                headerDvo.details = await _detailRepository
                    .AsQueryable()
                    .Where(a => a.header_id == id)
                    .Select<AdmFeedbackDetailDvo>()
                    .ToListAsync();
            }

            return headerDvo;
        }
    }
}
