using Com.Scm.Samples.Book;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm.Samples
{
    public static class SamplesServerUtils
    {
        public static void Setup(IServiceCollection services)
        {
            services.AddScoped<IBookService, SamplesBookService>();
        }
    }
}
