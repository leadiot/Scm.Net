using Com.Scm.Helper;
using Com.Scm.Utils;
using System.Reflection;

namespace Com.Scm.Samples.Helper
{
    public class SamplesDbHelper : ScmDbHelper
    {
        private const string KEY = "Samples.Net";
        /// <summary>
        /// 数据版本
        /// </summary>
        private const int VER = 5;
        /// <summary>
        /// 发行日期
        /// </summary>
        private const string DATE = "2026-05-12";


        public SamplesDbHelper()
        {
            //ScmServerHelper.Register(new SamplesDbHelper());
        }

        public override bool DropDb()
        {
            return DropTable(Assembly.GetExecutingAssembly());
        }

        public override bool InitDb()
        {
            var key = KEY;

            var verDao = ReadDbVer(key);
            if (verDao == null)
            {
                verDao = new ScmVerDao();
                verDao.key = key;
                verDao.create_time = TimeUtils.GetUnixTime();

                InitDdl(verDao);

                InitDml(verDao);
            }
            else
            {
                // DDL处理
                UpgradeDdl(verDao);

                // DML处理
                UpgradeDml(verDao);
            }

            verDao.ver = VER;
            verDao.date = DATE;
            verDao.update_time = TimeUtils.GetUnixTime();
            SaveDbVer(verDao);
            return true;
        }

        protected override void InitDdl(ScmVerDao verDao)
        {
            // 表格处理
            InitTable(Assembly.GetExecutingAssembly());
        }

        protected override void InitDml(ScmVerDao verDao)
        {
            CreateUid(1000000000000002001, "samples_book", 1, "", "");
            CreateUid(1000000000000002002, "samples_po_header", 10, "PO", "");
        }

        protected override void UpgradeDdl(ScmVerDao verDao)
        {
            // 版本较新，不执行DDL
            if (verDao.ver < VER)
            {
                var ddlFile = Path.Combine(_SqlDir, "samples-ddl.sql");
                ExecuteSql(ddlFile, verDao.ver);
            }

            // 表格处理
            InitTable(Assembly.GetExecutingAssembly());
        }

        protected override void UpgradeDml(ScmVerDao verDao)
        {
            // 版本较新，不执行DML
            if (verDao.ver < VER)
            {
                var dmlFile = Path.Combine(_SqlDir, "samples-dml.sql");
                ExecuteSql(dmlFile, verDao.ver);
            }
        }
    }
}
