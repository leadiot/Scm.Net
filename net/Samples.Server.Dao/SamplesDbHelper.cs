using Com.Scm.Utils;
using System.Reflection;

namespace Com.Scm.Samples
{
    public class SamplesDbHelper : ScmDbHelper
    {
        private const int MAJOR = 10;
        private const int MINOR = 0;
        private const int PATCH = 0;
        private const string BUILD = "2026010101";
        private const string RELEASE_DATE = "2026-01-01";

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
            var key = "scm.samples";

            var verDao = ReadDbVer(key);
            if (verDao == null)
            {
                verDao = new ScmVerDao();
                verDao.key = key;
                verDao.create_time = TimeUtils.GetUnixTime();
            }

            InitTable(Assembly.GetExecutingAssembly());

            if (verDao.major == 0)
            {
                InitDml();
            }

            var ddlFile = Path.Combine(_BaseDir, "ddl-samples.sql");
            ExecuteSql(ddlFile, verDao.major);

            var dmlFile = Path.Combine(_BaseDir, "dml-samples.sql");
            ExecuteSql(dmlFile, verDao.major);

            verDao.major = MAJOR;
            verDao.minor = MINOR;
            verDao.patch = PATCH;
            verDao.build = BUILD;
            verDao.release_date = RELEASE_DATE;
            verDao.update_time = TimeUtils.GetUnixTime();
            SaveDbVer(verDao);
            return true;
        }

        private void InitDml()
        {
            CreateUid(1000000000000002001, "samples_book", 1, "", "");
            CreateUid(1000000000000002002, "samples_po_header", 10, "PO", "");
        }
    }
}
