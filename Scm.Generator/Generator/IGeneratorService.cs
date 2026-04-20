using Com.Scm.Generator.Dvo;
using Com.Scm.Utils;
using SqlSugar;

namespace Com.Scm.Generator
{
    public interface IGeneratorService
    {
        /// <summary>
        /// 查询所有表
        /// </summary>
        /// <returns></returns>
        List<DbTableInfo> GetTable(string key);

        /// <summary>
        /// 根据表名查询列信息
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        List<DbColumnInfo> GetColumn(string table);

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool CreateCode(GeneratorTableRequest request);

        GenHelper Helper { get; }

        Dictionary<string, List<string>> GetOptions();

        string Message { get; }
    }
}