using Ehrms.EmployeeInfo.API.Behaviour;
using Ehrms.EmployeeInfo.API.Middleware;
using FluentValidation;
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

		services.AddValidatorsFromAssembly(assembly);

		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(assembly)
				.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		});

		services.AddAutoMapper(assembly);

		return services;
	}
}
