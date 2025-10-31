using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Sys.Dic;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Service
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class ScmDicService : ApiService, IDicService
    {
        private readonly SugarRepository<DicHeaderDao> _headerRepository;
        private readonly SugarRepository<DicDetailDao> _detailRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerRepository"></param>
        /// <param name="detailRepository"></param>
        public ScmDicService(SugarRepository<DicHeaderDao> headerRepository, SugarRepository<DicDetailDao> detailRepository)
        {
            _headerRepository = headerRepository;
            _detailRepository = detailRepository;
        }

        /// <summary>
        /// 获取字典信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("{key}")]
        public async Task<List<DicOptionDvo>> OptionAsync(string key)
        {
            var headerDao = await _headerRepository
                .GetFirstAsync(a => a.codec == key && a.row_status == Enums.ScmRowStatusEnum.Enabled);
            if (headerDao != null)
            {
                return await _detailRepository
                      .AsQueryable()
                      .Where(a => a.dic_header_id == headerDao.id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                      .OrderBy(a => a.od, SqlSugar.OrderByType.Asc)
                      .Select(a => new DicOptionDvo { id = a.id, label = a.namec, value = a.value, codec = a.codec, cat = a.cat })
                      .ToListAsync();
            }

            return new List<DicOptionDvo>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<DicHeaderDao> GetDicAsync(string key)
        {
            var headerDao = await _headerRepository.GetFirstAsync(a => a.codec == key && a.row_status == Enums.ScmRowStatusEnum.Enabled);
            if (headerDao != null)
            {
                headerDao.details = await _detailRepository.GetListAsync(a => a.dic_header_id == headerDao.id && a.row_status == Enums.ScmRowStatusEnum.Enabled, a => a.od, Enums.OrderByEnum.Asc);
            }
            return headerDao;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DicHeaderDao GetDic(string key)
        {
            var headerDao = _headerRepository.GetFirst(a => a.codec == key && a.row_status == Enums.ScmRowStatusEnum.Enabled);
            if (headerDao != null)
            {
                headerDao.details = _detailRepository.GetList(a => a.dic_header_id == headerDao.id && a.row_status == Enums.ScmRowStatusEnum.Enabled, a => a.od, Enums.OrderByEnum.Asc);
            }
            return headerDao;
        }
    }
}
