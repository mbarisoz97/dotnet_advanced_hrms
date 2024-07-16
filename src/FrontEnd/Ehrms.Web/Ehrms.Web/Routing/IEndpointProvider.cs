namespace Ehrms.Web;

internal interface IEndpointProvider
{
	string AutheticationEndpoint { get; }
	string RefreshEndpoint { get; }
	string EmployeeInfoServiceEndpoint { get; }
	string? ProjectManagementApiEndpoint { get; }
	string? EmploymentApiEndpoint { get; }
	string? TrainingManagementServiceEndpoint { get; }
}
