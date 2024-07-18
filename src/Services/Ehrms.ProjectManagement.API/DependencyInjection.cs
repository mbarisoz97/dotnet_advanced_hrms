using Ehrms.Contracts.Skill;
using Ehrms.ProjectManagement.API.Consumer.EmployeeEvents;
using Ehrms.ProjectManagement.API.Consumer.SkillEvents;
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
        services.AddScoped<ProjectManagementDatabaseSeed>();
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


    internal static IBusRegistrationConfigurator AddEventConsumers(this IBusRegistrationConfigurator busConfigurator)
    {
		busConfigurator.AddConsumer<EmployeeCreatedConsumer>();
		busConfigurator.AddConsumer<EmployeeUpdatedConsumer>();
		busConfigurator.AddConsumer<EmployeeDeletedConsumer>();

		busConfigurator.AddConsumer<SkillCreatedEventConsumer>();
		busConfigurator.AddConsumer<SkillUpdatedEventConsumer>();
		busConfigurator.AddConsumer<SkillDeletedEventConsumer>();

		return busConfigurator;

	}
}