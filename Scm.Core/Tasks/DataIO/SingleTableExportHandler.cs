using Com.Scm.Cfg.Export;
using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Sys.Tasks;
using CsvHelper;
using SqlSugar;
using System.Data;
using System.Globalization;

namespace Com.Scm.Tasks.DataIO
{
    /// <summary>
    /// 
    /// </summary>
    public class SingleTableExportHandler : ITaskHandler
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Types { get { return 0; } }

        /// <summary>
        /// 
        /// </summary>
        public override string Name { get { return "用户数据导出"; } }

        /// <summary>
        /// 
        /// </summary>
        public override string Json { get { return ""; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        /// <param name="dao"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override void Execute(EnvConfig config, ISqlSugarClient client, TaskDao dao)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        /// <param name="dao"></param>
        public void Export(EnvConfig config, ISqlSugarClient client, TaskDao dao)
        {
            try
            {
                DoExport(config, client, dao);
                dao.result = ScmResultEnum.Success;
            }
            catch (Exception ex)
            {
                dao.result = ScmResultEnum.Failure;
                dao.message = ex.Message;
            }
        }

        private void DoExport(EnvConfig config, ISqlSugarClient client, TaskDao dao)
        {
            var exportHeaderDao = client.Queryable<ExportHeaderDao>().Where(a => a.id == 0).First();
            var exportDetailListDao = client.Queryable<ExportDetailDao>().Where(a => a.id == 0).ToList();

            //OpenXmlConfiguration colConfig = null;
            //if (exportDetailListDao != null)
            //{
            //    colConfig = new OpenXmlConfiguration();
            //    var columns = new DynamicExcelColumn[exportDetailListDao.Count];
            //    for (int i = 0; i < exportDetailListDao.Count; i++)
            //    {
            //        var exportDetailDao = exportDetailListDao[i];
            //        columns[i] = new DynamicExcelColumn(exportDetailDao.col) { Name = exportDetailDao.col, Index = i };
            //    }
            //}

            var table = client.Ado.GetDataReader(dao.json);
            var path = config.GetTempPath(exportHeaderDao.file);
            //MiniExcel.SaveAs(path, table, configuration: colConfig);
            using (var writer = new StreamWriter("foo.csv"))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    Saveas(csv, table);
                }
            }
        }

        private void Saveas(CsvWriter writer, IDataReader reader)
        {

        }
    }
}
