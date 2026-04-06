using Com.Scm.Controllers;
using Com.Scm.Filters;
using Com.Scm.Generator;
using Com.Scm.Generator.Config;
using Com.Scm.Generator.Dvo;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Api.Controllers
{
    /// <summary>
    /// 代码生成
    /// </summary>
    [ApiExplorerSettings(GroupName = "Scm")]
    public class GeneratorController : ApiController
    {
        private readonly IGeneratorService _generatorService;
        private GeneratorConfig _Config;

        public GeneratorController(IGeneratorService generatorService, GeneratorConfig config)
        {
            _generatorService = generatorService;
            _Config = config;
        }

        [HttpGet("option")]
        public Dictionary<string, List<string>> OptionAsync()
        {
            return _generatorService.GetOptions();
        }

        [HttpGet("table")]
        public List<DbTableInfo> TableAsync(string key)
        {
            return _generatorService.GetTable(key);
        }


        [HttpGet("column")]
        public ScmSearchPageResponse<DbColumnInfo> ColumnAsync(string table)
        {
            return new ScmSearchPageResponse<DbColumnInfo>()
            {
                Items = _generatorService.GetColumn(table)
            };
        }

        [HttpPost, NoJsonResult]
        public IActionResult Post([FromBody] GeneratorTableRequest request)
        {
            if (_generatorService.CreateCode(request))
            {
                if (_Config.Download)
                {
                    return File(_generatorService.Helper.Bytes, "application/zip", "code_" + DateTime.Now.Ticks + ".zip");
                }

                return Ok("下载完成！");
            }
            return BadRequest(_generatorService.Message);
        }
    }
}