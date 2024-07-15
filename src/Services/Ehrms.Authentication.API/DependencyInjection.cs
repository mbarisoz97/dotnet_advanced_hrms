using Ehrms.Authentication.API.Database.Context;
using Ehrms.Shared;
using Microsoft.EntityFrameworkCore;

namespace Ehrms.Authentication.API;

internal static class DependencyInjection 
{
    internal static IServiceCollection AddAuthenticationApi(this IServiceCollection services)
    {
        services.AddScoped<ApplicationUserDbSeed>();
        services.AddScoped<DbContext,ApplicationUserDbContext>();
        services.AddTransient<ITokenHandler, JwtTokenHandler>();
        return services;
    }
}