namespace Ehrms.Web;

internal sealed class EndpointProvider : IEndpointProvider
{
	public string RefreshEndpoint => "/api/Account/Refresh";
	public string AutheticationEndpoint => "/api/Account/Login";
	public string UserEndpoint => "/api/User";
	public string EmployeeSkillServiceEndpoint => "/api/Skill";
    public string EmployeeInfoServiceEndpoint => "/api/Employee";
	public string? ProjectManagementApiEndpoint => "/api/Project";
	public string? EmploymentApiEndpoint => "/api/Project/Employment";
	
	public string? TrainingManagementServiceEndpoint => "/api/Training";
	public string? TrainingRecommendationServiceEndpoint => "/api/TrainingRecommendation";
}
