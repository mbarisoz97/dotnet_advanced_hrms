using MediatR;
using FluentValidation;
using System.Reflection;
using Ehrms.Administration.API.PipelineBehaviors;
using Ehrms.Administration.API.Middleware;
using Ehrms.Administration.API.Database.Context;

namespace Ehrms.Administration.API;

public static class DependencyInjection
{
	public static IServiceCollection AddAdministrationApi(this IServiceCollection services)
	{
		services.AddAssemblyTypes();
		services.AddThirdPartyLibraryConfigurations();

		return services;
	}

	private static IServiceCollection AddAssemblyTypes(this IServiceCollection services)
	{
		services.AddScoped<DbContext, AdministrationDbContext>();
		services.AddScoped<GlobalExceptionHandlingMiddleware>();

		return services;
	}

	private static IServiceCollection AddThirdPartyLibraryConfigurations(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();

		services.AddValidatorsFromAssemblyContaining(typeof(Program), includeInternalTypes: true);

		services.AddMediatR(config =>
		{
			config.RegisterServicesFromAssembly(assembly);
			config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		});

		services.AddAutoMapper(assembly);

		return services;
	}

    internal static IBusRegistrationConfigurator AddEventConsumers(this IBusRegistrationConfigurator busConfigurator)
    {
        busConfigurator.AddConsumer<EmployeeCreatedEventConsumer>();
        busConfigurator.AddConsumer<EmployeeUpdatedEventConsumer>();
        busConfigurator.AddConsumer<EmployeeDeletedEventConsumer>();
        return busConfigurator;
    }
}