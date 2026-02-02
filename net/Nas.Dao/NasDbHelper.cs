using Com.Scm.Utils;
using System.Reflection;

namespace Com.Scm.Nas
{
    public class NasDbHelper : ScmDbHelper
    {
        public NasDbHelper()
        {
            //ScmServerHelper.Register(new NasDbHelper());
        }

        public override bool DropDb()
        {
            return DropTable(Assembly.GetExecutingAssembly());
        }

        public override bool InitDb()
        {
            var key = "scm.nas";

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

            var ddlFile = Path.Combine(_BaseDir, "ddl-nas.sql");
            ExecuteSql(ddlFile, verDao.major);

            var dmlFile = Path.Combine(_BaseDir, "dml-nas.sql");
            ExecuteSql(dmlFile, verDao.major);

            SaveDbVer(verDao);
            return true;
        }

        private void InitDml()
        {
        }
    }
}
