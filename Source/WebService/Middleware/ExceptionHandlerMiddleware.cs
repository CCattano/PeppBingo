using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Text;
using System.Threading.Tasks;
using WebException = Pepp.Web.Apps.Bingo.Infrastructure.Exceptions.WebException;

namespace Pepp.Web.Apps.Bingo.WebService.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpCtx)
        {
            try
            {
                await _next(httpCtx);
            }
            catch (WebException webEx)
            {
                // TODO: Write to Db
                httpCtx.Response.StatusCode = (int)webEx.ResponseCode;
                await httpCtx.Response.WriteAsync(webEx.Message);
            }
            catch
            {
                // TODO: Write to db before allowing Http500 to bubble out
                throw;
            }
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
