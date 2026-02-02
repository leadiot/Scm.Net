using Com.Scm.Utils;
using System.Reflection;

namespace Com.Scm.Samples
{
    public class SamplesDbHelper : ScmDbHelper
    {
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

            SaveDbVer(verDao);
            return true;
        }

        private void InitDml()
        {
        }
    }
}
