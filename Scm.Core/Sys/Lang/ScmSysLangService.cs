using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Service;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Lang
{
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysLangService : ApiService
    {
        private readonly SugarRepository<LangDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="config"></param>
        public ScmSysLangService(SugarRepository<LangDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<TextOptionDvo>> GetOptionAsync()
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .Select(a => new TextOptionDvo { id = a.id, label = a.text, value = a.code })
                .ToListAsync();
        }
    }
}
