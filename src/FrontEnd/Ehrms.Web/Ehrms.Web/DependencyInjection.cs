using Ehrms.Shared;
using Ehrms.Web.Client;
using Ehrms.Web.Client.AuthApi;
using Ehrms.Web.Client.AuthApi.Account;
using Ehrms.Web.Client.EmployeeApi.Info;
using Ehrms.Web.Client.EmployeeApi.Skill;
using Ehrms.Web.Client.EmployeeApi.Title;
using Ehrms.Web.Client.ProjectApi.Employment;
using Ehrms.Web.Client.ProjectApi.ProjectInfo;
using Ehrms.Web.Client.TrainingApi.Info;
using Ehrms.Web.Client.TrainingApi.Preference;
using Ehrms.Web.Client.TrainingApi.Recommendation;
using Ehrms.Web.Routing;

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
        services.AddScoped<IAccountServiceClient, AccountServiceClient>();
        services.AddScoped<IEmployeeTitleClient, EmployeeTitleClient>();
        services.AddScoped<ITrainingPreferenceClient, TrainingPreferenceClient>();

        services.AddScoped<IUserServiceClient, UserServiceClient>();
        services.AddScoped<IUserRoleServiceClient, UserRoleServiceClient>();

        services.AddScoped<ITokenHandler, JwtTokenHandler>();
        services.AddScoped<IHttpClientFactoryWrapper, HttpClientFactoryWrapper>();

        return services;
    }
}