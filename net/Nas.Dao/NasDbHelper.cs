using Com.Scm.Dao;
using Com.Scm.Utils;
using SqlSugar;
using System.Reflection;

namespace Com.Scm.Nas
{
    public class NasDbHelper : ScmDbHelper
    {
        public NasDbHelper(ISqlSugarClient sqlClient) : base(sqlClient)
        {
        }

        public override void InitDdl()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var scmDao = typeof(ScmDao);
            var daoType = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType && u.Name.EndsWith("Dao")).ToList();
            var daoList = new List<Type>();
            foreach (var item in daoType.Where(s => !s.IsInterface))
            {
                if (CommonUtils.HasImplementedRawGeneric(item, scmDao))
                {
                    daoList.Add(item);
                }
            }
            _SqlClient.CodeFirst.InitTables(daoList.ToArray());
        }

        public override void InitDml()
        {

        }
    }
}
