using Ehrms.EmployeeInfo.API.Middleware;
using MassTransit;
using System.Reflection;

namespace Ehrms.EmployeeInfo.API;

public static class DependencyInjection
{
    public static IServiceCollection AddEmployeeInfoApi(this IServiceCollection services)
    {
        services.AddAssemblyTypes();
        services.AddThirdPartLibraries();
        return services;
    }

    private static IServiceCollection AddAssemblyTypes(this IServiceCollection services)
    {
        services.AddScoped<DbContext, EmployeeInfoDbContext>();
        services.AddTransient<GlobalExceptionHandlingMiddleware>();

        return services;
    }

    private static IServiceCollection AddThirdPartLibraries(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });
        services.AddAutoMapper(assembly);

        return services;
    }
}
