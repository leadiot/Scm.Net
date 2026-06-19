using Com.Scm.Token;
using Com.Scm.Utils;
using SqlSugar;

public class Program
{
    public static void Main(string[] args)
    {
        var t = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        Console.WriteLine(t);
    }
}
