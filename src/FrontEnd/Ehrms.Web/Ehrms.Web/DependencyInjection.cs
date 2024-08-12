using Ehrms.Shared;
using Ehrms.Web.Client;

namespace Ehrms.Web;

internal static class DependencyInjection
{
    internal static IServiceCollection AddWebUi(this IServiceCollection services)
    {
        services.AddSingleton<IEndpointProvider, EndpointProvider>();

        services.AddScoped<IEmploymentServiceClient, EmploymentServiceClient>();
        services.AddScoped<IProjectServiceClient, ProjectServiceClient>();
        services.AddScoped<IEmployeeServiceClient, EmployeeInfoServiceClient>();
        services.AddScoped<ITrainingRecommendationServiceClient, TrainingRecommendationServiceClient>();
        services.AddScoped<ISkillServiceClient, SkillServiceClient>();
        services.AddScoped<ITrainingServiceClient, TrainingServiceClient>();
        services.AddScoped<IUserServiceClient, UserServiceClient>();
        services.AddScoped<ITokenHandler, JwtTokenHandler>();
        services.AddScoped<IHttpClientFactoryWrapper, HttpClientFactoryWrapper>();

        return services;
    }
}