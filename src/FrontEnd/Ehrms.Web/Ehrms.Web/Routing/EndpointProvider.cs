namespace Ehrms.Web;

internal sealed class EndpointProvider : IEndpointProvider
{
	public string RefreshEndpoint => "/api/Account/Refresh";
	public string AutheticationEndpoint => "/api/Account/Login";
	public string EmployeeInfoServiceEndpoint => "/api/Skill";
	public string? ProjectManagementApiEndpoint => "/api/Project";
	public string? EmploymentApiEndpoint => "/api/Project/Employment";
	public string? TrainingManagementServiceEndpoint => "/api/Training";
}
