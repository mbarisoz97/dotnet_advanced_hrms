using System.Reflection;

namespace Ehrms.TrainingManagement.API;

internal static class DependencyInjection
{
	internal static IServiceCollection AddTrainingManagementApi(this IServiceCollection services)
	{
		AddThirdPartyLibraries(services);
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