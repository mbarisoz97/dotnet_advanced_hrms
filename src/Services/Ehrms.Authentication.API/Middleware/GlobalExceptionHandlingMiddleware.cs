using System.Net;
using Ehrms.Shared.Exceptions;

namespace Ehrms.Authentication.API.Middleware;

internal class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (CustomNotFoundException)
        {
            _logger.LogInformation("Caught custom not found exception.");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
        catch (CustomAlreadyInUseException inUseException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(inUseException.Message);
        }
        catch (ValidationException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync(validationException.Message);
        }
    }
}