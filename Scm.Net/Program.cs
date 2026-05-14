using Com.Scm.Configure.Startup;

namespace Com.Scm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApplication.CreateBuilder(args)
                .ConfigureServices()
                .Build()
                .ConfigureMiddleware()
                .Run();
        }
    }
}
