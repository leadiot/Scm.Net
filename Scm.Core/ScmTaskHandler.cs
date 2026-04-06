using Com.Scm.Cfg.Export;
using CsvHelper;
using System.Globalization;

namespace Com.Scm
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ScmTaskHandler : ITaskHandler
    {
        /// <summary>
        /// 
        /// </summary>
        protected void SaveByRaw<T>(string file, List<T> list)
        {
            using (var writer = new StreamWriter(file))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    WriteHead<T>(csv);
                    csv.NextRecord();

                    WriteData(csv, list);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csv"></param>
        protected void WriteHead<T>(CsvWriter csv)
        {
            csv.WriteHeader<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csv"></param>
        /// <param name="records"></param>
        protected void WriteData<T>(CsvWriter csv, IEnumerable<T> records)
        {
            csv.WriteRecords(records);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="csv"></param>
        /// <param name="list"></param>
        protected void WriteHead(CsvWriter csv, List<ExportDetailDao> list)
        {
            foreach (var item in list)
            {
                csv.WriteField(item.namec);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="list"></param>
        /// <param name="obj"></param>
        protected void WriteData(CsvWriter writer, List<ExportDetailDao> list, object obj)
        {
            var fields = obj.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var item in list)
            {
                var key = item.col;
                if (string.IsNullOrEmpty(key))
                {
                    writer.WriteField(item.def);
                    continue;
                }

                var field = fields.Where(a => a.Name != null && a.Name.Equals(key, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (field == null)
                {
                    writer.WriteField(item.def);
                    continue;
                }

                var name = field.PropertyType.FullName;
                var val = field.GetValue(obj);
                switch (name)
                {
                    case "System.String":
                        writer.WriteField((string)val);
                        break;
                    case "System.Int32":
                        writer.WriteField((int)val);
                        break;
                    case "System.Int64":
                        writer.WriteField((long)val);
                        break;
                    case "System.Double":
                        writer.WriteField((double)val);
                        break;
                    case "System.Single":
                        writer.WriteField((float)val);
                        break;
                    case "System.DateTime":
                        writer.WriteField((DateTime)val);
                        break;
                }
            }
        }
    }
}
