using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Filters;
using Com.Scm.Res.Cat;
using Com.Scm.Service;
using Com.Scm.Sys.Notes.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Notes
{
    /// <summary>
    /// 记事
    /// </summary>
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysNoteService : ApiService
    {
        private readonly SugarRepository<NoteDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="resHolder"></param>
        /// <param name="config"></param>
        public ScmSysNoteService(SugarRepository<NoteDao> thisRepository,
            IResHolder resHolder,
            EnvConfig config)
        {
            _thisRepository = thisRepository;
            _ResHolder = resHolder;
            _EnvConfig = config;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<NoteBasicDvo>> GetPagesAsync(NoteSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(IsValidId(request.cat_id), a => a.cat_id == request.cat_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .WhereIF(request.types != NoteTypesEnum.None, a => a.types == request.types)
                .OrderBy(m => m.id)
                .Select<NoteBasicDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<NoteBasicDvo>> GetListAsync(NoteSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(IsValidId(request.cat_id), a => a.cat_id == request.cat_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .WhereIF(request.types != NoteTypesEnum.None, a => a.types == request.types)
                .OrderBy(m => m.id, SqlSugar.OrderByType.Desc)
                .Select<NoteBasicDvo>()
                .ToListAsync();

            //Prepare(result);
            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NoteDvo> GetAsync(long id)
        {
            var dvo = new NoteDvo();

            var dao = await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .FirstAsync();

            if (dao != null)
            {
                dvo.id = dao.id;
                dvo.types = dao.types;
                dvo.title = dao.title;
                dvo.content = dao.summary;
                dvo.ver = dao.ver;

                if (dao.files > 0)
                {
                    var content = _EnvConfig.ReadFile(NoteDto.FOLDER_NAME, dao.GetFileName());
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        dvo.content = content;
                    }
                }
            }

            return dvo;
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NoteDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<NoteDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NoteDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<NoteDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(NoteDto model)
        {
            var dao = model.Adapt<NoteDao>();
            if (IsValidId(dao.cat_id))
            {
                dao.cat_id = ScmResCatDto.SYS_ID;
            }

            dao.files = model.IsTooLong() ? 1 : 0;
            dao.summary = model.ToDbSummary();
            dao.content = model.ToDbContent();

            var qty = await _thisRepository.InsertAsync(dao);

            SaveFile(dao, model);

            return qty;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<NoteDto> SaveAsync(NoteDto model)
        {
            NoteDao dao = null;
            var tooLong = model.IsTooLong();

            if (IsNormalId(model.id))
            {
                dao = await _thisRepository.GetByIdAsync(model.id);
            }

            if (dao == null)
            {
                dao = new NoteDao();
                dao.id = model.id;
                dao.types = model.types;
                dao.title = model.title;
                dao.cat_id = model.cat_id;
                dao.summary = model.ToDbSummary();
                dao.content = model.ToDbContent();
                dao.files = tooLong ? 1 : 0;
                await _thisRepository.InsertAsync(dao);

                model.id = dao.id;
            }
            else
            {
                dao.title = model.title;
                dao.cat_id = model.cat_id;
                dao.summary = model.ToDbSummary();
                dao.content = model.ToDbContent();
                dao.files = tooLong ? 1 : 0;
                await _thisRepository.UpdateAsync(dao);
            }

            SaveFile(dao, model);

            model.ver = dao.ver;
            model.update_time = dao.update_time;
            model.create_time = dao.create_time;
            return model;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(NoteDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return;
            }

            dao = model.Adapt(dao);
            if (!IsValidId(dao.cat_id))
            {
                dao.cat_id = ScmResCatDto.SYS_ID;
            }

            dao.files = model.IsTooLong() ? 1 : 0;
            dao.summary = model.ToDbSummary();
            dao.content = model.ToDbContent();

            await _thisRepository.UpdateAsync(dao);

            SaveFile(dao, model);
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
        [HttpPost, AllowAnonymous, NoJsonResult]
        public async Task<NoteUploadResponse> UploadAsync([FromForm] ScmUploadRequest request)
        {
            var response = new NoteUploadResponse();

            //判断是否上传了文件内容
            if (request.file == null)
            {
                response.SetFailure("上传内容为空！");
                return response;
            }

            #region 保存文件
            var fileName = request.file.FileName;
            var ext = Path.GetExtension(fileName);
            fileName = DateTime.UtcNow.Ticks.ToString() + ext;

            var path = _EnvConfig.GetUploadPath(fileName);
            using (var stream = File.OpenWrite(path))
            {
                //将文件内容复制到流中
                await request.file.CopyToAsync(stream);
            }

            response.SetSuccess(_EnvConfig.ToUri(path));
            #endregion

            return response;
        }

        private void SaveFile(NoteDao dao, NoteDto dto)
        {
            if (dto.IsTooLong())
            {
                _EnvConfig.SaveFile(NoteDto.FOLDER_NAME, dao.GetFileName(), dto.content);
            }
        }
    }
}
