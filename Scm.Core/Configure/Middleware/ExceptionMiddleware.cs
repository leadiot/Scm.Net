using Com.Scm.Response;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Com.Scm.Configure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ExceptionHandlerAsync(context, ex);
            }
        }

        private async Task ExceptionHandlerAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            var result = new ScmApiResponse()
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = ex.Message
            };

            await context.Response.WriteAsync(result.ToJsonString());
        }
    }
}