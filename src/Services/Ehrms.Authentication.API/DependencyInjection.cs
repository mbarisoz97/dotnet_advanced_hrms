using Ehrms.Shared;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Ehrms.Authentication.API.Adapter;
using Ehrms.Authentication.API.Middleware;
using Ehrms.Authentication.API.Database.Context;
using Ehrms.Authentication.API.PipelineBehaviors;

namespace Ehrms.Authentication.API;

internal static class DependencyInjection 
{
    internal static IServiceCollection AddAuthenticationApi(this IServiceCollection services)
    {
        services.AddScoped<ApplicationUserDbSeed>();
        services.AddScoped<DbContext,ApplicationUserDbContext>();
        services.AddTransient<ITokenHandler, JwtTokenHandler>();
        services.AddScoped<IUserManagerAdapter, UserManagerAdapter>();

        services.AddScoped<GlobalExceptionHandlingMiddleware>();

        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly)
             .AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        return services;
    }
}