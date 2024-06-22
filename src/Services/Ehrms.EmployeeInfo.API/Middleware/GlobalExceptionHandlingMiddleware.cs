using System.Net;

namespace Ehrms.EmployeeInfo.API.Middleware;

internal class GlobalExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch(CustomNotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        catch
        {
            throw;
        }
    }
}
