using Ehrms.ProjectManagement.API.PipelineBehaviors;
using FluentValidation;
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

    private static void AddAssemblyTypes(IServiceCollection services)
    {
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
    }

    private static void AddThirdPartyLibraries(IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(assembly);
		services.AddValidatorsFromAssembly(assembly);

		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(assembly)
				.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		});
    }
}