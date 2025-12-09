using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Samples.Demo.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;

namespace Com.Scm.Samples.Demo
{
    /// <summary>
    /// 示例代码服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Samples")]
    public class SamplesDemoService : ApiService
    {
        private readonly SugarRepository<DemoDao> _thisRepository;
        private readonly EnvConfig _Config;
        private readonly IUserService _userService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="thisRepository"></param>
        public SamplesDemoService(SugarRepository<DemoDao> thisRepository, IUserService userService, EnvConfig config)
        {
            _thisRepository = thisRepository;
            _userService = userService;
            _Config = config;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<DemoDvo>> GetPageAsync(SearchRequest request)
        {
            var isEmpty = string.IsNullOrWhiteSpace(request.key);
            var isCodes = !isEmpty && SamplesUtils.IsDemoCodes(request.key);

            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_delete != ScmDeleteEnum.Yes)
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(IsNormalId(request.option_id), a => a.option_id == request.option_id)
                .WhereIF(isCodes, a => a.codes == request.key)
                .WhereIF(!isEmpty && !isCodes, a => a.codec.Contains(request.key) || a.names.Contains(request.key))
                .OrderByDescending(a => a.id)
                .Select<DemoDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<DemoDvo>> GetListAsync(SearchRequest request)
        {
            var items = await _thisRepository.AsQueryable()
                .Where(a => a.row_delete != ScmDeleteEnum.Yes)
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(IsNormalId(request.option_id), a => a.option_id == request.option_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.codec.Contains(request.key) || a.names.Contains(request.key))
                .OrderByDescending(a => a.id)
                .Select<DemoDvo>()
                .ToListAsync();

            Prepare(items);
            return items;
        }

        private void Prepare(List<DemoDvo> list)
        {
            foreach (var item in list)
            {
                item.update_names = _userService.GetUserNames(item.update_user);
                item.create_names = _userService.GetUserNames(item.create_user);

                // Others TODO
            }
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<DemoDvo> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<DemoDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<DemoDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<DemoDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddAsync(DemoDto model)
        {
            await _thisRepository.InsertAsync(model.Clone<DemoDao>());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(DemoDto model)
        {
            await _thisRepository.UpdateAsync(model.Clone<DemoDao>());
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus(_thisRepository, param.ids, param.status);
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

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ScmUploadResponse> UploadAsync([FromForm] ScmUploadRequest request)
        {
            var response = new ScmUploadResponse();

            //判断是否上传了文件内容
            if (request.file == null)
            {
                response.SetFailure("上传内容为空！");
                return response;
            }

            #region 保存文件
            var fileName = request.file.FileName;
            var ext = Path.GetExtension(fileName);
            fileName = System.DateTime.UtcNow.Ticks.ToString() + ext;

            var path = _Config.GetUploadPath(fileName);
            using (var stream = File.OpenWrite(path))
            {
                //将文件内容复制到流中
                await request.file.CopyToAsync(stream);
            }

            response.AddResult(new ScmUploadResult { file = fileName });
            #endregion

            response.SetSuccess("文件上传成功！");
            return response;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImportResponse> ImportAsync([FromForm] ImportRequest request)
        {
            var response = new ImportResponse();

            //判断是否上传了文件内容
            if (request.file == null)
            {
                response.SetFailure("上传内容为空！");
                return response;
            }

            #region 数据导入
            int qty = 0;
            using (var stream = request.file.OpenReadStream())
            {
                var list = stream.Query<DemoExcelDvo>();
                foreach (var item in list)
                {
                    var dao = item.Clone<DemoDao>();
                    await _thisRepository.InsertAsync(dao);
                }
                qty = list.Count();
            }
            response.qty = qty;
            response.SetSuccess("文件导入成功！");
            #endregion

            return response;
        }

        /// <summary>
        /// 查看文件
        /// </summary>
        /// <param name="file">文件代码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileResult> GetFileAsync(string file)
        {
            var path = _Config.GetUploadPath(file);
            using (var stream = File.OpenRead(path))
            {
                var bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, bytes.Length);
                return new FileContentResult(bytes, "image/png");
            }
        }
    }
}
