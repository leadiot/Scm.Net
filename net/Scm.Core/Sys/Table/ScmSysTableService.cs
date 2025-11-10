using Com.Scm.Dsa;
using Com.Scm.Jwt;
using Com.Scm.Service;
using Com.Scm.Sys.Table.Dvo;
using Com.Scm.Utils;

using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Table
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "v1")]
    public class ScmSysTableService : ApiService
    {
        private readonly JwtContextHolder _contextHolder;
        private readonly SugarRepository<SysTableHeaderDao> _headerRepository;
        private readonly SugarRepository<SysTableDetailDao> _detailRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextHolder"></param>
        /// <param name="headerRepository"></param>
        /// <param name="detailRepository"></param>
        /// <returns></returns>
        public ScmSysTableService(JwtContextHolder contextHolder,
            SugarRepository<SysTableHeaderDao> headerRepository,
            SugarRepository<SysTableDetailDao> detailRepository)
        {
            _contextHolder = contextHolder;
            _headerRepository = headerRepository;
            _detailRepository = detailRepository;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="codec"></param>
        /// <returns></returns>
        [HttpGet("{codec}")]
        public async Task<SysTableHeaderDto> GetAsync(string codec)
        {
            var token = _contextHolder.GetToken();

            var dto = await _headerRepository.AsQueryable()
                .ClearFilter()
                .Where(a => a.codec == codec && a.user_id == token.user_id)
                .Select<SysTableHeaderDto>()
                .FirstAsync();
            if (dto != null)
            {
                dto.details = await _detailRepository.AsQueryable()
                    .Where(a => a.header_id == dto.id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                    .OrderBy(a => a.od, SqlSugar.OrderByType.Asc)
                    .Select<SysTableDetailDto>()
                    .ToListAsync();
            }
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> PostSaveAsync(SaveRequest request)
        {
            var token = _contextHolder.GetToken();

            var dao = await _headerRepository.AsQueryable()
                .ClearFilter()
                .Where(a => a.codec == request.codec && a.user_id == token.user_id)
                .FirstAsync();
            if (dao == null)
            {
                dao = request.Adapt<SysTableHeaderDao>();
                await _headerRepository.InsertAsync(dao);
            }
            //else
            //{
            //    //dao.names = request.names;
            //    await _headerRepository.UpdateAsync(dao);
            //}

            await _detailRepository.AsDeleteable()
                .Where(a => a.header_id == dao.id)
                .ExecuteCommandAsync();
            if (request.details != null)
            {
                var items = new List<SysTableDetailDao>();
                var idx = 0;
                foreach (var detail in request.details)
                {
                    var item = detail.Adapt<SysTableDetailDao>();
                    item.header_id = dao.id;
                    item.od = idx++;
                    items.Add(item);
                }
                await _detailRepository.InsertRangeAsync(items);
            }

            return true;
        }
    }
}
