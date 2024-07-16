using System.Net;

namespace Ehrms.TrainingManagement.API.Middleware;

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
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
		catch (FluentValidation.ValidationException validationException)
		{
			context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			await context.Response.WriteAsync(validationException.Message);
		}
        catch(Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occured.");
        }
    }
}