using Ehrms.Shared;

namespace Ehrms.Authentication.API;

internal static class DependencyInjection 
{
    internal static IServiceCollection AddAuthenticationApi(this IServiceCollection services)
    {
        services.AddTransient<ITokenHandler, JwtTokenHandler>();
        return services;
    }
}