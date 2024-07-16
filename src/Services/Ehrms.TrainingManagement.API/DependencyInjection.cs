using Ehrms.TrainingManagement.API.Middleware;
using Ehrms.TrainingManagement.API.PipelineBehaviors;
using FluentValidation;
using System.Reflection;

namespace Ehrms.TrainingManagement.API;

internal static class DependencyInjection
{
	internal static IServiceCollection AddTrainingManagementApi(this IServiceCollection services)
	{
		services.AddScoped<TrainingDbSeed>();
		services.AddTransient<GlobalExceptionHandlingMiddleware>();
		AddThirdPartyLibraries(services);
		return services;
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