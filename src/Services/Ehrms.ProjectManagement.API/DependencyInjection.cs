using System.Reflection;

namespace Ehrms.ProjectManagement.API;

internal static class DependencyInjection
{
    internal static IServiceCollection AddProjectManagementApi(this IServiceCollection services)
    {
        AddAssemblyTypes(services);
        AddThirdPartyLibraries(services);

        return services;
    }

    private static IServiceCollection AddAssemblyTypes(IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddThirdPartyLibraries(IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(assembly);
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        return services;
    }
}