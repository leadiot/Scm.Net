using Com.Scm.Res.Ext;
using SqlSugar;

public class Program
{
    public static void Main(string[] args)
    {
        var sqlConfig = new ConnectionConfig()
        {
            DbType = SqlSugar.DbType.Sqlite,
            ConnectionString = "Data Source=D:/data/scm.db;",
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        };
        var sqlClient = new SqlSugarClient(sqlConfig);
        sqlClient.Open();

        var lines = File.ReadAllLines(@"D:\a.txt");
        var idx = 0;
        var id = 2019234152112591848;
        while (idx < lines.Length)
        {
            var line = lines[idx++];
            if (line.Trim() == "<tr>")
            {
                line = lines[idx++];
                if (line.Trim() == "<td>")
                {
                    line = lines[idx++];
                    var mime = GetText(line);
                    idx++;
                    line = lines[idx++];
                    var exts = GetText(line).TrimStart('.');
                    idx += 2;
                    //Console.WriteLine($"Exts:{exts},Mime:{mime}");

                    var extDao = sqlClient.Queryable<ScmResExtDao>().First(a => a.codec == exts);
                    if (extDao == null)
                    {
                        extDao = new ScmResExtDao();
                        extDao.codec = exts;
                        extDao.namec = exts + " 文件";
                        extDao.mime = mime;
                        extDao.sign = "";
                        extDao.id = id++;
                        sqlClient.Insertable(extDao).ExecuteCommand();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(extDao.mime))
                        {
                            extDao.mime = mime;
                            sqlClient.Updateable(extDao).ExecuteCommand();
                        }
                        else if (extDao.mime != mime)
                        {
                            Console.WriteLine($"后缀：{exts}，原MIME：{extDao.mime}，新MIME：{mime}");
                        }
                    }
                }
            }
        }
    }

    public static string GetText(string line)
    {
        var pre = "text-gray-500\">";
        var s = line.IndexOf(pre);
        if (s < 0)
        {
            return null;
        }
        s += pre.Length;

        var t = line.IndexOf("</a>");
        if (t < s)
        {
            return null;
        }
        return line.Substring(s, t - s);
    }
}
