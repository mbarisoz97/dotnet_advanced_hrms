using FluentValidation;
using System.Reflection;

namespace Ehrms.Administration.API;

internal static class DependencyInjection
{
	internal static IServiceCollection AddAdministrationApi(this IServiceCollection services)
	{
        var assembly = Assembly.GetExecutingAssembly();

        services.AddValidatorsFromAssembly(assembly);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });

        services.AddAutoMapper(assembly);

        return services;
	}
}
