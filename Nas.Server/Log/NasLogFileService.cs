using Com.Scm.Dsa;
using Com.Scm.Nas.Log.Dvo;
using Com.Scm.Service;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Nas.Log
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Nas")]
    public class NasLogFileService : ApiService
    {
        private readonly SugarRepository<NasLogFileDao> _thisRepository;
        private readonly IDicService _DicHolder;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public NasLogFileService(SugarRepository<NasLogFileDao> thisRepository, IDicService dicHolder, IResHolder resHolder)
        {
            _thisRepository = thisRepository;
            _DicHolder = dicHolder;
            _ResHolder = resHolder;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<NasLogFileDvo>> GetPagesAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(IsNormalId(request.terminal_id), a => a.terminal_id == request.terminal_id)
                .WhereIF(IsNormalId(request.drive_id), a => a.folder_id == request.drive_id)
                .WhereIF(request.opt != NasOptEnums.None, a => a.opt == request.opt)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(m => m.id, SqlSugar.OrderByType.Desc)
                .Select<NasLogFileDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        private void Prepare(List<NasLogFileDvo> items)
        {
            var dicType = _DicHolder.GetDic("nas_type");
            var dicDir = _DicHolder.GetDic("nas_dir");
            var dicOpt = _DicHolder.GetDic("nas_opt");
            foreach (var item in items)
            {
                item.type_name = dicType.GetDetailNamec((int)item.type);
                item.opt_name = dicDir.GetDetailNamec((int)item.opt);
                item.dir_name = dicOpt.GetDetailNamec((int)item.dir);
                item.terminal_name = _ResHolder.GetResNamec<ScmUrTerminalDao>(item.terminal_id);
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NasLogFileDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<NasLogFileDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NasLogFileDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<NasLogFileDvo>()
                .FirstAsync();
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> DeleteAsync(string ids)
        {
            return await DeleteRecord(_thisRepository, ids.ToListLong());
        }
    }
}