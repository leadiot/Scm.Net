using MiniExcelLibs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Scm.Utils
{
    public class ExcelUtils
    {
        public static async Task<List<T>> Read<T>(Stream stream) where T : class, new()
        {
            var query = await stream.QueryAsync<T>();
            return query.ToList();
        }
    }
}