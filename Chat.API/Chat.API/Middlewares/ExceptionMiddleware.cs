using System.Net;
using Chat.API.Exceptions;

namespace Chat.API.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BaseException ex)
        {
            context.Response.StatusCode = (int)ex.StatusCode;
            await context.Response.WriteAsJsonAsync(new { ex.Message });
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new { ex.Message });
        }
    }
}