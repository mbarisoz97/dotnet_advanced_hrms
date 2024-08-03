using FluentValidation;
using System.Reflection;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.EmployeeEvent;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.ProjectEvent;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.SkillEvent;
using Ehrms.TrainingManagement.API.MessageQueue.Consumers.TrainingEvents;
using Ehrms.TrainingManagement.API.MessageQueue.StateMachine;
using Ehrms.TrainingManagement.API.Middleware;
using Ehrms.TrainingManagement.API.PipelineBehaviors;

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

    internal static IBusRegistrationConfigurator AddEventConsumers(this IBusRegistrationConfigurator busConfigurator)
    {
        busConfigurator.AddConsumer<EmployeeCreatedEventConsumer>();
        busConfigurator.AddConsumer<EmployeeDeletedEventConsumer>();
        busConfigurator.AddConsumer<EmployeeUpdatedEventConsumer>();

        busConfigurator.AddConsumer<SkillCreatedEventConsumer>();
        busConfigurator.AddConsumer<SkillUpdatedEventConsumer>();
        busConfigurator.AddConsumer<SkillDeletedEventConsumer>();

        busConfigurator.AddConsumer<ProjectCreatedEventConsumer>();
        busConfigurator.AddConsumer<ProjectUpdatedEventConsumer>();

        busConfigurator.AddConsumer<TrainingRecommendationCompletedEventConsumer>();
        busConfigurator.AddConsumer<TrainingRecommendationRequestAcceptedEventConsumer>();
        
        return busConfigurator;
    }
}