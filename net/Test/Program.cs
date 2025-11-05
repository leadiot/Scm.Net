using Com.Scm.Utils;

public class Program
{
    public static void Main(string[] args)
    {
        var result = SecUtils.AesEncrypt("123456", "@Scm.Net");
        Console.WriteLine(result);

        result = SecUtils.AesDecrypt(result, "@Scm.Net");
        Console.WriteLine(result);
    }
}
