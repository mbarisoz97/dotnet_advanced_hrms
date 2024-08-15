namespace Ehrms.Web;

internal interface IEndpointProvider
{
	string AutheticationEndpoint { get; }
	string RefreshEndpoint { get; }
	string EmployeeSkillServiceEndpoint { get; }
	string? ProjectManagementApiEndpoint { get; }
	string? EmploymentApiEndpoint { get; }
	string? TrainingManagementServiceEndpoint { get; }
	string? TrainingRecommendationServiceEndpoint { get; }
	string EmployeeInfoServiceEndpoint { get; }
    string UserEndpoint { get; }
    string? UserRoleEndpoint { get; }
}
