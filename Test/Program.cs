using Com.Scm.Token;

public class Program
{
    public static void Main(string[] args)
    {
        var text = "scm123";
        var token = ScmToken.FromAppToken(text);
    }
}
