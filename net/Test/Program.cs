using Com.Scm;
using SqlSugar;

public class Program
{
    public static void Main(string[] args)
    {
        var sqlClient = new SqlSugarClient(new ConnectionConfig
        {
            DbType = DbType.Sqlite,
            ConnectionString = "Data Source=D:/data/scm1.db;"
        });
        sqlClient.Open();

        new ScmDbHelper(sqlClient).InitDb("");
    }
}
